using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;

namespace Assets.ACS.Scripts.Systems
{

    public partial class ACS_ProjectileSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem ecbSystem;

        BuildPhysicsWorld buildPhysicsWorldSystem;
        StepPhysicsWorld stepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            buildPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        }

        [BurstCompile]
        private struct CollisionEventJob : ICollisionEventsJob
        {
            public void Execute(CollisionEvent collisionEvent)
            {
                Debug.Log("Collision");
                //entityCommandBuffer.DestroyEntity(collisionEvent.EntityA);
            }
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();
            float2 verticalEdges = ACS_GameManager.Instance.verticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.horizontalEdges;

            CollisionWorld collisionWorld = buildPhysicsWorldSystem.PhysicsWorld.CollisionWorld;

            Entities.ForEach((ref Translation translation, ref ACS_ProjectileData projectileData, in Entity entity) =>
            {
                projectileData.TimeSinceFired += deltaTime;
                
                if (projectileData.IsExpired)
                    entityCommandBuffer.DestroyEntity(entity);
                else
                {
                    // Keep the projectile within the screen boundaries
                    float offset = 0.5f;
                    if (translation.Value.z > verticalEdges.y)
                    {
                        translation.Value.z = verticalEdges.x + offset;
                    }
                    else
                    {
                        if (translation.Value.z < verticalEdges.x)
                        {
                            translation.Value.z = verticalEdges.y - offset;
                        }
                    }
                    if (translation.Value.x > horizontalEdges.y)
                    {
                        translation.Value.x = horizontalEdges.x + offset;
                    }
                    else
                    {
                        if (translation.Value.x < horizontalEdges.x)
                        {
                            translation.Value.x = horizontalEdges.y - offset;
                        }
                    }

                    // Makre sure no funny physics mess with the z plane
                    if (translation.Value.y != 0)
                        translation.Value.y = 0;

                }
            }).Schedule();

            CompleteDependency();
        }

    }
}
