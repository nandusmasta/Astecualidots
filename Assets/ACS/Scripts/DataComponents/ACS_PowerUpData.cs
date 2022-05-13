/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_PowerUpData : IComponentData
    {

        public enum PowerUpType { Invulnerability, MegaBomb, RepairKit };
        public PowerUpType type;
        public float TimeToLive;
        public float TimeSinceSpawned;

        public bool IsExpired
        {
            get
            {
                return TimeSinceSpawned > TimeToLive;
            }
        }

    }
}
