using UnityEngine;
using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_ShipMovementData : IComponentData
    {

        public float rotation;
        public float speed;

    }
}
