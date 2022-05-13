/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Entities;

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
        public Entity RepairKitPowerUp;
        public Entity InvulnerabilityPowerUp;
        public Entity MegaBombPowerUp;

    }
}
