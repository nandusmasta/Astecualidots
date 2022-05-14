/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;
    using Unity.Mathematics;

    [GenerateAuthoringComponent]
    public struct ACS_EnemyData : IComponentData
    {
        #region Fields

        public float Health;

        public float2 MinMaxInitialSpeed;

        public Entity Projectile;

        public float RateOfFire;

        public float TimeSinceLastShot;

        public int ScoreWorth;

        public Entity explosion;

        #endregion

        #region Properties

        public bool IsDestroyed
        {
            get
            {
                return Health <= 0;
            }
        }

        public bool IsReloading
        {
            get
            {
                return TimeSinceLastShot < RateOfFire;
            }
        }

        public bool HasExplosion
        {
            get
            {
                return explosion != Entity.Null;
            }
        }

        #endregion
    }
}
