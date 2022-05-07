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
        private EntityCommandBufferSystem entityCommandBufferSystem;
        protected override void OnCreate() =>
            entityCommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

        protected override void OnUpdate()
        {

            var commandBuffer = entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.WithStructuralChanges().ForEach((ref ACS_ShipMovementData shipMovementData, in ACS_ShipInputData shipInputData, in ACS_ShipData shipData, in LocalToWorld shipTransform) =>
            {
                
                shipMovementData.rotation += Input.GetKey(shipInputData.turnLeftKey) ? -shipData.rotationAcceleration : 0f;
                shipMovementData.rotation += Input.GetKey(shipInputData.turnRightKey) ? shipData.rotationAcceleration : 0f;
                shipMovementData.speed += Input.GetKey(shipInputData.accelerateKey) ? shipData.acceleration : 0;
                shipMovementData.speed += Input.GetKey(shipInputData.decelerateKey) ? -shipData.acceleration : 0;

                shipMovementData.speed = Mathf.Clamp(shipMovementData.speed, -shipData.maxSpeed, shipData.maxSpeed);
                shipMovementData.rotation = Mathf.Clamp(shipMovementData.rotation, -shipData.maxRotationSpeed, shipData.maxRotationSpeed);

                // Shoot
                if (Input.GetKeyUp(shipInputData.shootKey))
                {
                    Entity newBullet = EntityManager.Instantiate(shipData.Bullet);
                    Translation newBulletTranslation = new Translation { Value = shipTransform.Position + (shipTransform.Forward * 5f)};
                    PhysicsVelocity newBulletPhysicsVelocity = new PhysicsVelocity { Linear = shipTransform.Forward * shipData.BulletSpeed };
                    commandBuffer.SetComponent(newBullet.Index, newBullet, newBulletTranslation);
                    commandBuffer.SetComponent(newBullet.Index, newBullet, newBulletPhysicsVelocity);
                }

            }).Run();
        }
    }
}
