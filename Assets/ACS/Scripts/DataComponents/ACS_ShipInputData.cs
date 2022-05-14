/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;
    using UnityEngine;

    [GenerateAuthoringComponent]
    public struct ACS_ShipInputData : IComponentData
    {
        #region Fields

        public KeyCode accelerateKey;

        public KeyCode decelerateKey;

        public KeyCode shootKey;

        public KeyCode turnLeftKey;

        public KeyCode turnRightKey;

        #endregion
    }
}
