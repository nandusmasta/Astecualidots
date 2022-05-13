/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Entities;
using UnityEngine.UI;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public class ACS_UIData : IComponentData
    {
        public Text Health;
        public Text Score;
    }
}
