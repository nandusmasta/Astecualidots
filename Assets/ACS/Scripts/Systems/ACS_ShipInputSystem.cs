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
    using Unity.Physics;
    using Unity.Transforms;
    using UnityEngine;

    [AlwaysSynchronizeSystem]
    public partial class ACS_ShipInputSystem : SystemBase
    {
        #region Fields

        private float eulerAngleTripleShoot = 25f;

        private float timeSinceLastShoot;

        private float xzOffsetForShooting = 8f;

        #endregion

        #region Methods

        protected override void OnUpdate()
        {
            if (!ACS_Globals.IsPlayerPlaying) return;
            float deltaTime = Time.DeltaTime;
            timeSinceLastShoot += deltaTime;

            Entities.ForEach((ref ACS_ShipMovementData shipMovementData, ref ACS_ShipData shipData,
                in ACS_ShipInputData shipInputData, in LocalToWorld shipTransform, in Rotation shipRotation, in Translation shipTranslation) =>
            {
                // Facing
                if (Input.GetKey(shipInputData.turnLeftKey))
                    shipMovementData.RotationForce = -shipData.rotationAcceleration;
                else
                    if (Input.GetKey(shipInputData.turnRightKey))
                    shipMovementData.RotationForce = shipData.rotationAcceleration;

                // Speed
                if (Input.GetKey(shipInputData.accelerateKey))
                    shipMovementData.VelocityForce = shipData.acceleration;
                else
                    if (Input.GetKey(shipInputData.decelerateKey))
                    shipMovementData.VelocityForce = -shipData.acceleration;

                // Shooting
                bool isReloading = timeSinceLastShoot < shipData.RateOfFire;
                if (!isReloading && Input.GetKey(shipInputData.shootKey))
                {
                    Entity newProjectile = EntityManager.Instantiate(shipData.Projectile);
                    ACS_ProjectileData projectileData = GetComponent<ACS_ProjectileData>(newProjectile);
                    projectileData.TimeSinceFired = 0f;
                    Translation newProjectileTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * xzOffsetForShooting) };
                    PhysicsVelocity newProjectilePhysicsVelocity = new PhysicsVelocity { Linear = shipTransform.Forward * projectileData.Speed };
                    SetComponent(newProjectile, shipRotation);
                    SetComponent(newProjectile, newProjectileTranslation);
                    SetComponent(newProjectile, newProjectilePhysicsVelocity);
                    timeSinceLastShoot = 0f;

                    // Triple shoot implementation
                    if (shipData.HasTripleShoot)
                    {
                        // Left projectile
                        Entity leftProjectile = EntityManager.Instantiate(shipData.Projectile);
                        ACS_ProjectileData leftProjectileData = GetComponent<ACS_ProjectileData>(newProjectile);
                        leftProjectileData.TimeSinceFired = 0f;
                        Translation leftProjectileTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * xzOffsetForShooting) };
                        Rotation leftProjectileRotation = new Rotation { Value = math.mul(shipRotation.Value, Quaternion.AngleAxis(-eulerAngleTripleShoot, Vector3.up)) };
                        PhysicsVelocity leftProjectilePhysicsVelocity = new PhysicsVelocity { Linear = math.forward(leftProjectileRotation.Value) * projectileData.Speed };
                        SetComponent(leftProjectile, leftProjectileRotation);
                        SetComponent(leftProjectile, leftProjectileTranslation);
                        SetComponent(leftProjectile, leftProjectilePhysicsVelocity);

                        // Right projectile
                        Entity rigthProjectile = EntityManager.Instantiate(shipData.Projectile);
                        ACS_ProjectileData rigthProjectileData = GetComponent<ACS_ProjectileData>(newProjectile);
                        rigthProjectileData.TimeSinceFired = 0f;
                        Translation rigthProjectileTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * xzOffsetForShooting) };
                        Rotation rigthProjectileRotation = new Rotation { Value = math.mul(shipRotation.Value, Quaternion.AngleAxis(eulerAngleTripleShoot, Vector3.up)) };
                        PhysicsVelocity rigthProjectilePhysicsVelocity = new PhysicsVelocity { Linear = math.forward(rigthProjectileRotation.Value) * projectileData.Speed };
                        SetComponent(rigthProjectile, rigthProjectileRotation);
                        SetComponent(rigthProjectile, rigthProjectileTranslation);
                        SetComponent(rigthProjectile, rigthProjectilePhysicsVelocity);
                    }

                    // Play fire effect
                    ACS_GameAudioManager.Instance.PlayBlasterFiredSFX(false);
                }

                // Invulnerability
                if (shipData.IsInvulnerable)
                {
                    shipData.TimeSinceInvulnerable += deltaTime;
                    if (shipData.HasInvulnerabilityExpired)
                    {
                        shipData.IsInvulnerable = false;
                        shipData.TimeSinceInvulnerable = 0f;
                        EntityManager.SetEnabled(shipData.ShieldEffect, false);
                        shipData.IsShieldOn = false;
                    }
                    else
                    {
                        // Add shield
                        if (!shipData.IsShieldOn)
                        {
                            EntityManager.SetEnabled(shipData.ShieldEffect, true);
                            shipData.IsShieldOn = true;
                        }
                    }
                }

                // Invulnerability
                if (shipData.HasTripleShoot)
                {
                    shipData.TimeSinceTripleShoot += deltaTime;
                    if (shipData.HasTripleShootExpired)
                    {
                        shipData.HasTripleShoot = false;
                        shipData.TimeSinceTripleShoot = 0f;
                    }
                }

                // Spawn the explosion and destroy the entity
                if (shipData.IsDestroyed && shipData.HasExplosion)
                {
                    Entity explosion = EntityManager.Instantiate(shipData.explosion);
                    EntityManager.SetComponentData<Translation>(explosion, shipTranslation);

                    // Play SFX
                    ACS_GameAudioManager.Instance.PlayEnemyExplosionSFX();
                }

            }).WithStructuralChanges().Run();

            CompleteDependency();
        }

        #endregion
    }
}
