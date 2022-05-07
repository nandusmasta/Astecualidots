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
        public float BulletStrength;
        public float BulletDuration;
        public float BulletSpeed;
        public float RateOfFire;

        public int WasFiring;
        public int IsFiring;

    }
}
