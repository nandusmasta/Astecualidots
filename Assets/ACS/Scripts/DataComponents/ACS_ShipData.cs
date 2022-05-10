using UnityEngine;
using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_ShipData : IComponentData
    {
        public float Health;
        public float maxSpeed;
        public float acceleration;
        public float rotationAcceleration;
        public float maxAngularSpeed;

        public Entity Projectile;
        public float RateOfFire;

    }
}
