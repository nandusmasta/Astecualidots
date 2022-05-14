/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;

    [GenerateAuthoringComponent]
    public struct ACS_GameData : IComponentData
    {
        #region Fields

        public bool IsGameOver;

        public Entity LargeAsteroidPrefab;

        public Entity MediumAsteroidPrefab;

        public Entity SmallAsteroidPrefab;

        public Entity InvulnerabilityPowerUp;

        public Entity MegaBombPowerUp;

        public Entity RepairKitPowerUp;

        public Entity TripleShootPowerUp;

        public Entity Enemy1;

        public Entity Enemy2;

        public Entity Enemy3;

        public Entity ship;

        public int SpawnedLargeAsteroids;

        #endregion
    }
}
