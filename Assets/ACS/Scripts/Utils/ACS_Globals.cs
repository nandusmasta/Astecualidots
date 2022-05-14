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

        public static bool HasFiredMegaBomb = false;

        public static bool HasGameStarted = false;

        public static int Score;

        public static int SpawnedLargeAsteroids;

        public static float2 MinMaxEnemyVelocity = new float2 { x = 10, y = 15 };

        public static int2 MinimumSecondsBetweenEnemies = new int2 { x = 45, y = 75 };

        public static int MaxAsteroidsOnScreen = 7;

        public static float2 HorizontalEdges = new float2 { x = -213, y = 213 };

        public static Vector2 VerticalEdges = new float2 { x = -122, y = 122 };

        public static int2 MinimumSecondsBetweenPowerups = new int2 { x = 30, y = 60 };

        #endregion
    }
}
