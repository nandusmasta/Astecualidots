/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;

    [GenerateAuthoringComponent]
    public struct ACS_PowerUpData : IComponentData
    {
        #region Fields

        public float TimeSinceSpawned;

        public float TimeToLive;

        public PowerUpType type;

        public bool PickedUp;

        #endregion

        #region Enums

        public enum PowerUpType { Invulnerability, MegaBomb, RepairKit, MultiShoot };

        #endregion

        #region Properties

        public bool IsExpired
        {
            get
            {
                return TimeSinceSpawned > TimeToLive;
            }
        }

        #endregion
    }
}
