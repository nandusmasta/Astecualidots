/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using UnityEngine;
using Assets.ACS.Scripts.DataComponents;
using Assets.ACS.Scripts.Utils;
using UnityEngine.UI;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Assets.ACS.Scripts.Behaviours
{
    public class ACS_UIManager : ACS_MonoBehaviour
    {

        public RectTransform EndGamePanel;
        public Text Score;

        private static ACS_UIManager _Instance;
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

        public void Awake()
        {
            EndGamePanel.gameObject.SetActive(false);
            ACS_Globals.HasGameStarted = true;
        }

        public void ShowGameEndScreen(string score)
        {
            EndGamePanel.gameObject.SetActive(true);
            Score.text = score;
        }

        public void Restart()
        {
            ClearScene();
            SceneManager.LoadScene("ACS_Game");
        }

        public void MainMenu()
        {
            ClearScene();
            SceneManager.LoadScene("ACS_Menu");
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

    }
}
