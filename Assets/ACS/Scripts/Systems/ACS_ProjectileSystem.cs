using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_ProjectileSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            Entities.ForEach((ref ACS_ProjectileData projectileData, in Entity entity) =>
            {
                projectileData.TimeSinceFired += deltaTime;
                
                if (projectileData.IsExpired)
                    ecb.DestroyEntity(entity);
            }).Schedule();
            CompleteDependency();
        }
    }
}
