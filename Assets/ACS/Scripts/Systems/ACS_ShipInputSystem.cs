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
            Entities.ForEach((ref ACS_ShipMovementData shipMovementData, in ACS_ShipInputData shipInputData) =>
            {
                
                shipMovementData.direction += Input.GetKey(shipInputData.turnLeftKey) ? Vector3.up * -5 : Vector3.zero;
                shipMovementData.direction += Input.GetKey(shipInputData.turnRightKey) ? Vector3.up * 5 : Vector3.zero;
                shipMovementData.speed += Input.GetKey(shipInputData.accelerateKey) ? 0.1f : 0;
                shipMovementData.speed += Input.GetKey(shipInputData.decelerateKey) ? -0.1f : 0;

            }).Run();
        }
    }
}
