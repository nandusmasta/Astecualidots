/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Assets.ACS.Scripts.Utils;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_PowerUpSystem : SystemBase
    {

        BeginInitializationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;
            float deltaTime = Time.DeltaTime;
            float2 verticalEdges = ACS_GameManager.Instance.VerticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.HorizontalEdges;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();

            Entities.ForEach((ref ACS_PowerUpData powerUpData, ref Translation translation, in Entity powerUp) =>
            {
                powerUpData.TimeSinceSpawned += deltaTime;

                if (powerUpData.IsExpired)
                    entityCommandBuffer.DestroyEntity(powerUp);
                else
                {
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
                }
            }).Schedule();
        }
    }
}
