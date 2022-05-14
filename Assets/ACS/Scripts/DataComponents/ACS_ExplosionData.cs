/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;

    [GenerateAuthoringComponent]
    public struct ACS_ExplosionData : IComponentData
    {
        #region Fields

        public float TimeSinceFired;

        public float TimeToLive;

        #endregion

        #region Properties

        public bool IsExpired
        {
            get
            {
                return TimeSinceFired > TimeToLive;
            }
        }

        #endregion
    }
}
