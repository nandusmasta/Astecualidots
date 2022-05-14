/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{

    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Transforms;
    using UnityEngine;
    public partial class ACS_ExplosionSystem : SystemBase
    {

        #region Fields

        internal BeginInitializationEntityCommandBufferSystem ecbSystem;

        #endregion

        #region Methods

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();

            Entities.ForEach((ref ACS_ExplosionData explosionData, in Entity explosion) =>
            {
                explosionData.TimeSinceFired += deltaTime;

                if (explosionData.IsExpired)
                    entityCommandBuffer.DestroyEntity(explosion);
                

            }).Schedule();

            CompleteDependency();
        }

        #endregion

    }
}
