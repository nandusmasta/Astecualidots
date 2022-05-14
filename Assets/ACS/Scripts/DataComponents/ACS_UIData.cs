/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;
    using UnityEngine.UI;

    [GenerateAuthoringComponent]
    public class ACS_UIData : IComponentData
    {
        #region Fields

        public Text Health;

        public Text Score;

        #endregion
    }
}
