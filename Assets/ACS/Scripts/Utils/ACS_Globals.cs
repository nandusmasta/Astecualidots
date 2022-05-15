/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Utils
{
    using Unity.Mathematics;
    using UnityEngine;

    public static class ACS_Globals
    {
        #region Fields

        public static GameDifficulty Difficulty = GameDifficulty.Normal;

        public static bool HasFiredMegaBomb = false;

        public static bool IsPlayerPlaying = false;

        public static float2 HorizontalEdges = new float2 { x = -213, y = 213 };

        public static int MaxAsteroidsOnScreen = 5;

        public static int2 MinimumSecondsBetweenEnemies = new int2 { x = 30, y = 60 };

        public static int2 MinimumSecondsBetweenPowerups = new int2 { x = 20, y = 40 };

        public static float2 MinMaxEnemyVelocity = new float2 { x = 8, y = 12 };

        public static int Score;

        public static ShipType ShipTypeTofly = ShipType.Standard;

        public static int SpawnedLargeAsteroids;

        public static Vector2 VerticalEdges = new float2 { x = -122, y = 122 };

        #endregion

        #region Enums

        public enum GameDifficulty { Easy, Normal, Hard }

        public enum ShipType { Heavy, Standard, Fast }

        #endregion
    }
}
