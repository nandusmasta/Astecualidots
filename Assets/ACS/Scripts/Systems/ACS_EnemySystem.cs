/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Transforms;
    using UnityEngine;

    public partial class ACS_EnemySystem : SystemBase
    {
        #region Fields

        internal BeginInitializationEntityCommandBufferSystem ecbSystem;

        private Entity shipEntity;

        private float xzOffsetForShooting = 9f;

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

            if (shipEntity == Entity.Null)
            {
                shipEntity = GetEntityQuery(new ComponentType[] { typeof(ACS_ShipData) }).GetSingletonEntity();
            }

            Entities.ForEach((ref ACS_EnemyData enemyData, ref Translation translation, ref Rotation rotation, in Entity enemy) =>
            {
                // Keep the enemy within the screen boundaries
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

                // Make sure no funny physics mess with the z plane
                if (translation.Value.y != 0)
                    translation.Value.y = 0;
                rotation.Value.value.x = 0f;
                rotation.Value.value.z = 0f;

                if (!ACS_Globals.HasGameStarted) return;

                enemyData.TimeSinceLastShot += deltaTime;
                if (!enemyData.IsReloading && EntityManager.Exists(shipEntity))
                {
                    Entity newProjectile = EntityManager.Instantiate(enemyData.Projectile);
                    ACS_ProjectileData projectileData = EntityManager.GetComponentData<ACS_ProjectileData>(newProjectile);
                    projectileData.TimeSinceFired = 0f;

                    Translation shipTranslation = EntityManager.GetComponentData<Translation>(shipEntity);
                    quaternion toTargetRotationQuaternion = quaternion.LookRotation(shipTranslation.Value - translation.Value, Vector3.up);
                    float3 targetVector = math.mul(toTargetRotationQuaternion, Vector3.forward);
                    Translation newProjectileTranslation = new Translation { Value = translation.Value + (targetVector * xzOffsetForShooting) };
                    PhysicsVelocity newProjectilePhysicsVelocity = new PhysicsVelocity { Linear = targetVector * projectileData.Speed };
                    EntityManager.SetComponentData(newProjectile, new Rotation { Value = toTargetRotationQuaternion });
                    EntityManager.SetComponentData(newProjectile, newProjectileTranslation);
                    EntityManager.SetComponentData(newProjectile, newProjectilePhysicsVelocity);
                    enemyData.TimeSinceLastShot = 0f;
                }

                // Destroy the enemy if it's supposed to
                if (enemyData.IsDestroyed)
                {
                    // Spawn the explosion and destroy the entity
                    if (enemyData.HasExplosion)
                    {
                        Entity explosion = entityCommandBuffer.Instantiate(enemyData.explosion);
                        entityCommandBuffer.SetComponent<Translation>(explosion, EntityManager.GetComponentData<Translation>(enemy));
                    }
                    entityCommandBuffer.DestroyEntity(enemy);

                    // Ship update
                    if (EntityManager.Exists(shipEntity))
                    {
                        ACS_ShipData shipData = GetComponent<ACS_ShipData>(shipEntity);
                        shipData.Score += enemyData.ScoreWorth;
                        entityCommandBuffer.SetComponent(shipEntity, shipData);
                    }
                }

            }).WithStructuralChanges().Run();

            CompleteDependency();
        }

        #endregion
    }
}
