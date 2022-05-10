using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Assets.ACS.Scripts.Utils;
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
            float2 verticalEdges = ACS_GameManager.Instance.VerticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.HorizontalEdges;

            Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation, ref Translation translation, ref ACS_ShipMovementData shipMovementData,
                in ACS_ShipData shipData, in PhysicsMass physicsMass) =>
            {
                // Set linear velocity
                float3 direction = math.mul(rotation.Value, new float3(0f, 0f, 1f));
                physicsVelocity.Linear += ACS_Utils.ClampFloat3(direction * shipMovementData.VelocityForce * deltaTime, shipData.maxSpeed);
                shipMovementData.VelocityForce = 0f;

                // Set angular velocity
                float3 angularVelocity = new float3(0f, 1f, 0f) * math.radians(shipMovementData.RotationForce * deltaTime);
                quaternion inertiaOrientationInWorldSpace = math.mul(rotation.Value, physicsMass.InertiaOrientation);
                float3 angularVelocityInertiaSpace = math.rotate(math.inverse(inertiaOrientationInWorldSpace), angularVelocity);
                physicsVelocity.Angular += angularVelocityInertiaSpace;
                shipMovementData.RotationForce = 0f;

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

                // Clamp velocitied
                physicsVelocity.Linear = ACS_Utils.ClampFloat3(physicsVelocity.Linear, shipData.maxSpeed);
                physicsVelocity.Angular = ACS_Utils.ClampFloat3(physicsVelocity.Angular, shipData.maxAngularSpeed);

                // Makre sure no funny physics mess with the z plane
                if (translation.Value.y != 0)
                    translation.Value.y = 0;

            }).ScheduleParallel();

        }

    }
}
