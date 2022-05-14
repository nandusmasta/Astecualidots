/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.DataComponents
{
    using Unity.Entities;
    using Unity.Mathematics;

    [GenerateAuthoringComponent]
    public struct ACS_AsteroidData : IComponentData
    {
        #region Fields

        public int Damage;

        public Entity explosion;

        public float Health;

        public bool isStatic;

        public float2 MinMaxVelocityOnCreation;

        public int piecesToSpawnOnDestroy;

        public Entity pieceToSpawnOnDestroy;

        public int ScoreWorth;

        public AsteroidType type;

        #endregion

        #region Enums

        public enum AsteroidType { Large, Medium, Small };

        #endregion

        #region Properties

        public bool HasExplosion
        {
            get
            {
                return explosion != Entity.Null;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                return Health <= 0;
            }
        }

        public bool IsLarge
        {
            get
            {
                return type == AsteroidType.Large;
            }
        }

        public bool IsMedium
        {
            get
            {
                return type == AsteroidType.Medium;
            }
        }

        public bool IsSmall
        {
            get
            {
                return type == AsteroidType.Small;
            }
        }

        public bool SpawnsPieceOnDestroy
        {
            get
            {
                return pieceToSpawnOnDestroy != null && piecesToSpawnOnDestroy > 0;
            }
        }

        #endregion
    }
}
