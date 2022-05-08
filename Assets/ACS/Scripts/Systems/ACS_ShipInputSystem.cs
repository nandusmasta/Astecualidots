using UnityEngine;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Physics;
using Unity.Transforms;

namespace Assets.ACS.Scripts.Systems
{

    [AlwaysSynchronizeSystem]
    public partial class ACS_ShipInputSystem : SystemBase
    {

        private float timeSinceLastShoot;

        protected override void OnUpdate()
        {
            timeSinceLastShoot += Time.DeltaTime;

            Entities.WithStructuralChanges().ForEach((ref ACS_ShipMovementData shipMovementData, in ACS_ShipInputData shipInputData, in ACS_ShipData shipData, in LocalToWorld shipTransform) =>
            {
                // Facing
                shipMovementData.rotation += Input.GetKey(shipInputData.turnLeftKey) ? -shipData.rotationAcceleration : 0f;
                shipMovementData.rotation += Input.GetKey(shipInputData.turnRightKey) ? shipData.rotationAcceleration : 0f;

                // Speed
                shipMovementData.speed += Input.GetKey(shipInputData.accelerateKey) ? shipData.acceleration : 0;
                shipMovementData.speed += Input.GetKey(shipInputData.decelerateKey) ? -shipData.acceleration : 0;

                // Clamping
                shipMovementData.speed = Mathf.Clamp(shipMovementData.speed, -shipData.maxSpeed, shipData.maxSpeed);
                shipMovementData.rotation = Mathf.Clamp(shipMovementData.rotation, -shipData.maxRotationSpeed, shipData.maxRotationSpeed);

                // Shooting
                bool isReloading = timeSinceLastShoot < shipData.RateOfFire;
                if (!isReloading && Input.GetKey(shipInputData.shootKey))
                {
                    Entity newProjectile = EntityManager.Instantiate(shipData.Projectile);
                    ACS_ProjectileData projectileData = GetComponent<ACS_ProjectileData>(newProjectile);
                    projectileData.TimeSinceFired = 0f;
                    Translation newProjectileTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * 8f)};
                    PhysicsVelocity newProjectilePhysicsVelocity = new PhysicsVelocity { Linear = shipTransform.Forward * projectileData.Speed };
                    SetComponent(newProjectile, newProjectileTranslation);
                    SetComponent(newProjectile, newProjectilePhysicsVelocity);
                    timeSinceLastShoot = 0f;
                }

            }).Run();
        }
    }
}
