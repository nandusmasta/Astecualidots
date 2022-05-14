/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;

    [GenerateAuthoringComponent]
    public struct ACS_ShipMovementData : IComponentData
    {
        #region Fields

        public float RotationForce;

        public float VelocityForce;

        #endregion
    }
}
