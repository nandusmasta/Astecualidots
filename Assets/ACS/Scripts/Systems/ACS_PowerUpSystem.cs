/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.Behaviours;
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;

    public partial class ACS_PowerUpSystem : SystemBase
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
            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();

            Entities.ForEach((ref ACS_PowerUpData powerUpData, ref Translation translation, in Entity powerUp) =>
            {
                powerUpData.TimeSinceSpawned += deltaTime;

                if (powerUpData.IsExpired)
                    entityCommandBuffer.DestroyEntity(powerUp);
                else
                {
                    // Keep the power up within the screen boundaries
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

                    // Removed picked up power up
                    if (powerUpData.PickedUp)
                    {
                        EntityManager.DestroyEntity(powerUp);

                        // Play SFX
                        ACS_GameAudioManager.Instance.PlayPowerUpPickupSFX();
                    }

                }
            }).WithStructuralChanges().Run();
        }

        #endregion
    }
}
