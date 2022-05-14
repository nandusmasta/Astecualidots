/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Behaviours
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Entities;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class ACS_UIManager : ACS_MonoBehaviour
    {
        #region Fields

        public RectTransform EndGamePanel;

        public Text Score;

        private static ACS_UIManager _Instance;

        #endregion

        #region Properties

        public static ACS_UIManager Instance
        {
            get
            {
                if (_Instance != null)
                    return _Instance;
                _Instance = GameObject.Find("UIManager").GetComponent<ACS_UIManager>();
                return _Instance;
            }
        }

        #endregion

        #region Methods

        public void Awake()
        {
            EndGamePanel.gameObject.SetActive(false);
            ACS_Globals.HasGameStarted = true;
        }

        public void MainMenu()
        {
            ClearScene();
            SceneManager.LoadScene("ACS_Menu");
        }

        public void Restart()
        {
            ClearScene();
            SceneManager.LoadScene("ACS_Game");
        }

        public void ShowGameEndScreen(string score)
        {
            EndGamePanel.gameObject.SetActive(true);
            Score.text = score;
        }

        private void ClearScene()
        {
            ACS_Globals.HasGameStarted = false;
            ACS_Globals.SpawnedLargeAsteroids = 0;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery asteroidEntities = entityManager.CreateEntityQuery(typeof(ACS_AsteroidData), typeof(Collider));
            entityManager.DestroyEntity(asteroidEntities);
            ScriptBehaviourUpdateOrder.RemoveWorldFromCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
            DefaultWorldInitialization.Initialize("Default World", false);
        }

        #endregion
    }
}
