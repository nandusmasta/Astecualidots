/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using UnityEngine;
using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_ShipInputData : IComponentData
    {

        public KeyCode accelerateKey;
        public KeyCode turnLeftKey;
        public KeyCode turnRightKey;
        public KeyCode decelerateKey;
        public KeyCode shootKey;

    }
}
