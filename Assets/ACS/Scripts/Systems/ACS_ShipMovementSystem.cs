using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_ShipMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            float3 rotationAxis = new float3(0f, 0f, 1f);
            float2 verticalEdges = ACS_GameManager.Instance.verticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.horizontalEdges;

            Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation, ref Translation translation, 
                in ACS_ShipMovementData shipMovementData, in ACS_ShipData shipData, in PhysicsMass physicsMass) =>
            {
                // Set linear velocity
                float3 direction = math.mul(rotation.Value, new float3(0f, 0f, 1f));
                physicsVelocity.Linear += direction * shipMovementData.speed * deltaTime;

                // Set angular velocity
                float3 angularVelocity = new float3(0f, 1f, 0f) * math.radians(shipMovementData.rotation * deltaTime);
                quaternion inertiaOrientationInWorldSpace = math.mul(rotation.Value, physicsMass.InertiaOrientation);
                float3 angularVelocityInertiaSpace = math.rotate(math.inverse(inertiaOrientationInWorldSpace), angularVelocity);
                physicsVelocity.Angular = angularVelocityInertiaSpace;

                // Keep the ship within the screen boundaries
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

            }).ScheduleParallel();

        }

    }
}
