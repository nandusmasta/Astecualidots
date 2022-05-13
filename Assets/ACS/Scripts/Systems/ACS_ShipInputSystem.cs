/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using UnityEngine;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Physics;
using Unity.Transforms;
using Assets.ACS.Scripts.Utils;

namespace Assets.ACS.Scripts.Systems
{

    [AlwaysSynchronizeSystem]
    public partial class ACS_ShipInputSystem : SystemBase
    {

        private float timeSinceLastShoot;

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;
            timeSinceLastShoot += Time.DeltaTime;
            float deltaTime = Time.DeltaTime;

            Entities.WithStructuralChanges().ForEach((ref ACS_ShipMovementData shipMovementData, ref ACS_ShipData shipData, 
                in ACS_ShipInputData shipInputData, in LocalToWorld shipTransform, in Rotation shipRotation) =>
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
                    Translation newProjectileTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * 8f)};
                    PhysicsVelocity newProjectilePhysicsVelocity = new PhysicsVelocity { Linear = shipTransform.Forward * projectileData.Speed };
                    SetComponent(newProjectile, shipRotation);
                    SetComponent(newProjectile, newProjectileTranslation);
                    SetComponent(newProjectile, newProjectilePhysicsVelocity);
                    timeSinceLastShoot = 0f;
                }

                // Invulnerability
                if (shipData.IsInvulnerable)
                {
                    shipData.TimeSinceInvulnerable += deltaTime;
                    if (shipData.HasInvulnerabilityExpired)
                    {
                        shipData.IsInvulnerable = false;
                        shipData.TimeSinceInvulnerable = 0f;
                    }
                }
            }).Run();
        }
    }
}
