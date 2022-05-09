using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Assets.ACS.Scripts.Utils;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;
using UnityEngine;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_AsteroidSystem : SystemBase
    {

        BeginInitializationEntityCommandBufferSystem ecbSystem;
        Entity gameDataEntity;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            EntityQuery gameDataQuery = GetEntityQuery(typeof(ACS_GameData));
            gameDataEntity = gameDataQuery.ToEntityArray(Allocator.Temp)[0];

            /*Entities.ForEach((ref PhysicsVelocity physicsVelocity, in Rotation rotation, in ACS_AsteroidData asteroidData) =>
            {
                if (asteroidData.isStatic)
                    return;
                float randomLinearInitialVelocity = ACS_Utils.GetRandomFloat(asteroidData.MinMaxVelocityOnCreation.x, asteroidData.MinMaxVelocityOnCreation.y);
                float3 randomAngularInitialVelocity = new float3(0f, 1f, 0f) * math.radians(ACS_Utils.GetRandomFloat(0, 360));

                // Set linear and angular velocity
                float3 randomXZDirection = new float3(ACS_Utils.GetRandomFloat(-5f, 5f), 0f, ACS_Utils.GetRandomFloat(-5f, 5f));
                physicsVelocity.Linear += randomXZDirection * randomLinearInitialVelocity;
                physicsVelocity.Angular = randomAngularInitialVelocity;

            }).Run();*/

            CompleteDependency();
        }

        protected override void OnUpdate()
        {
            float2 verticalEdges = ACS_GameManager.Instance.verticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.horizontalEdges;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();

            Entities.ForEach((ref Translation translation, ref PhysicsVelocity physicsVelocity, in Rotation rotation, in ACS_AsteroidData asteroidData, in Entity entity) =>
            {
                // Destroy the asteroid if it's supposed to
                /*if (asteroidData.IsDestroyed)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    destroyedAsteroids++;
                }*/

                // Keep the asteroid  within the screen boundaries
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
            }).Schedule();
            
            CompleteDependency();
        }
    }
}
