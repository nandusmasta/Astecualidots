/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Burst;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Physics.Systems;
    using Unity.Transforms;
    using UnityEngine;

    public partial class ACS_ProjectileSystem : SystemBase
    {
        #region Fields

        internal BuildPhysicsWorld buildPhysicsWorldSystem;

        internal BeginInitializationEntityCommandBufferSystem ecbSystem;

        internal StepPhysicsWorld stepPhysicsWorldSystem;

        #endregion

        #region Methods

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            buildPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorldSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();
            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;
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

        #endregion

    }
}
