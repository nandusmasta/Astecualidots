using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;

namespace Assets.ACS.Scripts.Systems
{

    [AlwaysSynchronizeSystem]
    public partial class ACS_ShipInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref ACS_ShipMovementData shipMovementData, in ACS_ShipInputData shipInputData, in ACS_ShipData shipData) =>
            {
                
                shipMovementData.rotation += Input.GetKey(shipInputData.turnLeftKey) ? -shipData.rotationAcceleration : 0f;
                shipMovementData.rotation += Input.GetKey(shipInputData.turnRightKey) ? shipData.rotationAcceleration : 0f;
                shipMovementData.speed += Input.GetKey(shipInputData.accelerateKey) ? shipData.acceleration : 0;
                shipMovementData.speed += Input.GetKey(shipInputData.decelerateKey) ? -shipData.acceleration : 0;

                shipMovementData.speed = Mathf.Clamp(shipMovementData.speed, -shipData.maxSpeed, shipData.maxSpeed);
                shipMovementData.rotation = Mathf.Clamp(shipMovementData.rotation, -shipData.maxRotationSpeed, shipData.maxRotationSpeed);

            }).Run();
        }
    }
}
