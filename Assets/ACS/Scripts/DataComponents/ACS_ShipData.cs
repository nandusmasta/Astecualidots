/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

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
        public bool IsInvulnerable;
        public int Score;
        public bool HasTripleShoot;
        public float MaxHealth;
        public float InvulnerabilityDuration;
        public float TimeSinceInvulnerable;
        public bool IsDestroyed
        {
            get
            {
                return Health <= 0;
            }
        }
        public bool HasInvulnerabilityExpired
        {
            get
            {
                return TimeSinceInvulnerable >= InvulnerabilityDuration;
            }
        }

    }
}
