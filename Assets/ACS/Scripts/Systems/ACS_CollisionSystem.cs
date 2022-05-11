using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
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
                if (projectileDataCDFE.HasComponent(collisionEvent.EntityA)) 
                    projectile = collisionEvent.EntityA;
                else
                    if (projectileDataCDFE.HasComponent(collisionEvent.EntityB))
                        projectile =  collisionEvent.EntityB;

                Entity ship = Entity.Null;
                if (shipDataCDFE.HasComponent(collisionEvent.EntityA))
                    ship = collisionEvent.EntityA;
                else
                    if (shipDataCDFE.HasComponent(collisionEvent.EntityB))
                    ship = collisionEvent.EntityB;

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

                // Asteroid on ship collision
                if (asteroid != Entity.Null && ship != Entity.Null)
                {
                    ACS_AsteroidData asteroidData;
                    asteroidDataCDFE.TryGetComponent(asteroid, out asteroidData);
                    ACS_ShipData shipData;
                    shipDataCDFE.TryGetComponent(ship, out shipData);
                    if (shipData.Health > 0 && !shipData.IsInvulnerable)
                    {
                        shipData.Health -= asteroidData.Damage;
                        entityCommandBuffer.SetComponent(ship, shipData);

                        // Destroy ship if it has enough damage done to it
                        if (shipData.IsDestroyed)
                        {
                            entityCommandBuffer.DestroyEntity(ship);
                        }

                        Debug.Log($"{asteroidData.Damage} points of damage done to the ship! It's destroyed status: {shipData.IsDestroyed}");
                    }
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
                entityCommandBuffer = ecbSystem.CreateCommandBuffer()
            }
            .Schedule(stepPhysicsWorldSystem.Simulation, Dependency);

            CompleteDependency();
        }
    }
}
