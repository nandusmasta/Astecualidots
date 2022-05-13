/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Assets.ACS.Scripts.Utils;

namespace Assets.ACS.Scripts.Systems
{

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(ExportPhysicsWorld))]
    [UpdateBefore(typeof(EndFramePhysicsSystem))]
    public partial class ACS_CollisionSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem ecbSystem;
        private BuildPhysicsWorld buildPhysicsWorldSystem;
        private StepPhysicsWorld stepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            buildPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();

            RequireForUpdate(GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(PhysicsCollider) }
            }));
        }

        private struct CollisionEventJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<ACS_AsteroidData> asteroidDataCDFE;
            [ReadOnly] public ComponentDataFromEntity<ACS_ProjectileData> projectileDataCDFE;
            [ReadOnly] public ComponentDataFromEntity<ACS_ShipData> shipDataCDFE;
            [ReadOnly] public ComponentDataFromEntity<ACS_PowerUpData> powerUpDataCDFE;
            public EntityCommandBuffer entityCommandBuffer;
            public void Execute(CollisionEvent collisionEvent)
            {
                // Ignore asteroid on asteroid collisions
                if (asteroidDataCDFE.HasComponent(collisionEvent.EntityA) && asteroidDataCDFE.HasComponent(collisionEvent.EntityB))
                    return;
                // Ignore projectile on projectile collisions
                if (projectileDataCDFE.HasComponent(collisionEvent.EntityA) && projectileDataCDFE.HasComponent(collisionEvent.EntityB))
                    return;

                Entity asteroid = Entity.Null;
                if (asteroidDataCDFE.HasComponent(collisionEvent.EntityA))
                    asteroid = collisionEvent.EntityA;
                else
                    if (asteroidDataCDFE.HasComponent(collisionEvent.EntityB))
                        asteroid = collisionEvent.EntityB;

                Entity projectile = Entity.Null;
                Entity projectile2 = Entity.Null;
                if (projectileDataCDFE.HasComponent(collisionEvent.EntityA)) 
                    projectile = collisionEvent.EntityA;
                if (projectileDataCDFE.HasComponent(collisionEvent.EntityB))
                    if (projectile == Entity.Null)
                        projectile = collisionEvent.EntityB;
                    else
                        projectile2 = Entity.Null;

                Entity ship = Entity.Null;
                if (shipDataCDFE.HasComponent(collisionEvent.EntityA))
                    ship = collisionEvent.EntityA;
                else
                    if (shipDataCDFE.HasComponent(collisionEvent.EntityB))
                    ship = collisionEvent.EntityB;

                Entity powerUp = Entity.Null;
                if (powerUpDataCDFE.HasComponent(collisionEvent.EntityA))
                    powerUp = collisionEvent.EntityA;
                else
                    if (powerUpDataCDFE.HasComponent(collisionEvent.EntityB))
                    powerUp = collisionEvent.EntityB;

                // Projectile on asteroid collision
                if (projectile != Entity.Null && asteroid != Entity.Null)
                {
                    ACS_AsteroidData asteroidData;
                    asteroidDataCDFE.TryGetComponent(asteroid, out asteroidData);
                    ACS_ProjectileData projectileData;
                    projectileDataCDFE.TryGetComponent(projectile, out projectileData);
                    if (asteroidData.Health > 0)
                    {
                        asteroidData.Health -= projectileData.Damage;
                        //Debug.Log($"{projectileData.Damage} points of damage done to asteroid! It's destroyed status: {asteroidData.IsDestroyed}");
                        entityCommandBuffer.DestroyEntity(projectile);
                    }
                    entityCommandBuffer.SetComponent(asteroid, asteroidData);

                }

                // Ship collisions
                if (ship != Entity.Null)
                {
                    ACS_ShipData shipData;
                    shipDataCDFE.TryGetComponent(ship, out shipData);
                    int damage = 0;

                    // Ship on asteroid collision
                    if (asteroid != Entity.Null)
                    {
                        ACS_AsteroidData asteroidData;
                        asteroidDataCDFE.TryGetComponent(asteroid, out asteroidData);
                        damage = asteroidData.Damage;

                        // Destroy the asteroid
                        asteroidData.Health = 0;
                        entityCommandBuffer.SetComponent<ACS_AsteroidData>(asteroid, asteroidData);
                    }
                    
                    // Ship on projectile collision
                    if (projectile != Entity.Null)
                    {
                        ACS_ProjectileData projectileData;
                        projectileDataCDFE.TryGetComponent(projectile, out projectileData);
                        damage = projectileData.Damage;
                        entityCommandBuffer.DestroyEntity(projectile);
                    }

                    // Ship on power up collision
                    if (powerUp != Entity.Null)
                    {
                        ACS_PowerUpData powerUpData;
                        powerUpDataCDFE.TryGetComponent(powerUp, out powerUpData);
                        switch (powerUpData.type)
                        {
                            case ACS_PowerUpData.PowerUpType.RepairKit:
                                shipData.Health = shipData.MaxHealth;
                                entityCommandBuffer.SetComponent(ship, shipData);
                                break;
                            case ACS_PowerUpData.PowerUpType.MegaBomb:
                                ACS_Globals.HasFiredMegaBomb = true;
                                break;
                            case ACS_PowerUpData.PowerUpType.Invulnerability:
                                shipData.IsInvulnerable = true;
                                shipData.TimeSinceInvulnerable = 0f;
                                entityCommandBuffer.SetComponent(ship, shipData);
                                break;
                        }
                        entityCommandBuffer.DestroyEntity(powerUp);
                    }

                    // Register ship damage
                    if (shipData.Health > 0 && !shipData.IsInvulnerable)
                    {
                        shipData.Health -= damage;
                        entityCommandBuffer.SetComponent(ship, shipData);

                        // Destroy ship if it has enough damage done to it
                        if (shipData.IsDestroyed)
                        {
                            entityCommandBuffer.DestroyEntity(ship);
                        }
                    }
                }

                // Projectile on projectile collision
                if (projectile != Entity.Null && projectile2 != Entity.Null)
                {
                    entityCommandBuffer.DestroyEntity(projectile);
                    entityCommandBuffer.DestroyEntity(projectile2);
                }

                //Debug.Log("Collision registered");
            }
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            this.RegisterPhysicsRuntimeSystemReadOnly();
        }

        protected override void OnUpdate()
        {
            // Schedule physics check
            Dependency = new CollisionEventJob {
                asteroidDataCDFE = GetComponentDataFromEntity<ACS_AsteroidData>(true),
                projectileDataCDFE = GetComponentDataFromEntity<ACS_ProjectileData>(true),
                shipDataCDFE = GetComponentDataFromEntity<ACS_ShipData>(true),
                powerUpDataCDFE = GetComponentDataFromEntity<ACS_PowerUpData>(true),
                entityCommandBuffer = ecbSystem.CreateCommandBuffer()
            }
            .Schedule(stepPhysicsWorldSystem.Simulation, Dependency);

            CompleteDependency();
        }
    }
}
