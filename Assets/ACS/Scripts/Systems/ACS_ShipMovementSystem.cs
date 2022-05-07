using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;

namespace Assets.ACS.Scripts.Systems
{
    [AlwaysSynchronizeSystem]
    public partial class ACS_ShipMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            float maxSpeed = ACS_GameManager.Instance.maxSpeed;

            Entities.ForEach((ref Translation translation, in ACS_ShipMovementData shipMovementData) =>
            {
                translation.Value.z += Mathf.Clamp(shipMovementData.speed * deltaTime, -maxSpeed, maxSpeed);
            }).Run();

        }
    }
}
