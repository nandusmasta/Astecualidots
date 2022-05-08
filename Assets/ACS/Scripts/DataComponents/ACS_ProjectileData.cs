using Unity.Entities;

namespace Assets.ACS.Scripts.DataComponents
{
    [GenerateAuthoringComponent]
    public struct ACS_ProjectileData : IComponentData
    {

        public float Speed;
        public float Damage;
        public float TimeToLive;
        public float TimeSinceFired;

        public bool IsExpired
        {
            get
            {
                return TimeSinceFired > TimeToLive;
            }
        }

    }
}
