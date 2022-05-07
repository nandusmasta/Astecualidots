using UnityEngine;
using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_ShipData : IComponentData
    {

        public float maxSpeed;
        public float acceleration;
        public float rotationAcceleration;
        public float maxRotationSpeed;

        public Entity Bullet;
        public float Strength;
        public float RateOfFire;
        public float Duration;

        public int WasFiring;
        public int IsFiring;

    }
}
