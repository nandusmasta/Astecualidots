using Unity.Entities;
using Unity.Mathematics;

namespace Assets.ACS.Scripts.DataComponents
{

    [GenerateAuthoringComponent]
    public struct ACS_GameData : IComponentData
    {

        public Entity LargeAsteroidPrefab;
        public Entity MediumAsteroidPrefab;
        public Entity SmallAsteroidPrefab;
        public int SpawnedLargeAsteroids;
        public bool IsGameOver;

    }
}
