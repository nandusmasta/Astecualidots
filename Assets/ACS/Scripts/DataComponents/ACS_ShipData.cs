/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;

    [GenerateAuthoringComponent]
    public struct ACS_ShipData : IComponentData
    {
        #region Fields

        public float acceleration;

        public bool HasTripleShoot;

        public float Health;

        public float InvulnerabilityDuration;

        public bool IsInvulnerable;

        public float maxAngularSpeed;

        public float MaxHealth;

        public float maxSpeed;

        public Entity Projectile;

        public float RateOfFire;

        public float rotationAcceleration;

        public int Score;

        public float TimeSinceInvulnerable;

        public float TimeSinceTripleShoot;

        public float TripleShootDuration;

        public Entity ShieldEffect;

        public bool IsShieldOn;

        #endregion

        #region Properties

        public bool HasInvulnerabilityExpired
        {
            get
            {
                return TimeSinceInvulnerable >= InvulnerabilityDuration;
            }
        }

        public bool HasTripleShootExpired
        {
            get
            {
                return TimeSinceTripleShoot >= TripleShootDuration;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                return Health <= 0;
            }
        }

        #endregion
    }
}
