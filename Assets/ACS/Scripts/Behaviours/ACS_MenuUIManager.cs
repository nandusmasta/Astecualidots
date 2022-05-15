/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Behaviours
{
    using Assets.ACS.Scripts.Utils;
    using Unity.Mathematics;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class ACS_MenuUIManager : ACS_MonoBehaviour
    {
        #region Methods

        public void DifficultyValueChanged(Dropdown dropDown)
        {
            SetDifficulty((ACS_Globals.GameDifficulty)dropDown.value);
        }

        public void ShipValueChanged(Dropdown dropDown)
        {
            SetShipType((ACS_Globals.ShipType)dropDown.value);
        }

        public void Start()
        {
            SetDifficulty(ACS_Globals.GameDifficulty.Normal);
            SetShipType(ACS_Globals.ShipType.Standard);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("ACS_Game");
        }

        private void SetDifficulty(ACS_Globals.GameDifficulty difficulty)
        {
            ACS_Globals.Difficulty = difficulty;
            switch (difficulty)
            {
                case ACS_Globals.GameDifficulty.Easy:
                    ACS_Globals.MaxAsteroidsOnScreen = 3;
                    ACS_Globals.MinimumSecondsBetweenEnemies = new int2 { x = 60, y = 90 };
                    ACS_Globals.MinMaxEnemyVelocity = new float2 { x = 7, y = 12 };
                    break;
                case ACS_Globals.GameDifficulty.Normal:
                    ACS_Globals.MaxAsteroidsOnScreen = 5;
                    ACS_Globals.MinimumSecondsBetweenEnemies = new int2 { x = 30, y = 60 };
                    ACS_Globals.MinMaxEnemyVelocity = new float2 { x = 8, y = 13 };
                    break;
                case ACS_Globals.GameDifficulty.Hard:
                    ACS_Globals.MaxAsteroidsOnScreen = 7;
                    ACS_Globals.MinimumSecondsBetweenEnemies = new int2 { x = 20, y = 40 };
                    ACS_Globals.MinMaxEnemyVelocity = new float2 { x = 9, y = 14 };
                    break;
            }
        }

        private void SetShipType(ACS_Globals.ShipType shipType)
        {
            ACS_Globals.ShipTypeTofly = shipType;
        }

        #endregion
    }
}
