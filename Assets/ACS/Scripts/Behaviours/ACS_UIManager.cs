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

        public Text Timer;

        public Text TimeTaken;

        private static ACS_UIManager _Instance;

        private float secondsSinceStart;

        public GameObject FastShip;

        public GameObject StandardShip;

        public GameObject HeavyShip;

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
            switch (ACS_Globals.ShipTypeTofly)
            {
                case ACS_Globals.ShipType.Fast: FastShip.SetActive(true); break;
                case ACS_Globals.ShipType.Heavy: HeavyShip.SetActive(true); break;
                case ACS_Globals.ShipType.Standard: StandardShip.SetActive(true); break;
            }
            ACS_Globals.IsPlayerPlaying = true;
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
            ACS_Globals.IsPlayerPlaying = false;
            EndGamePanel.gameObject.SetActive(true);
            Score.text = score;
            TimeTaken.text = Timer.text;
        }

        public void Update()
        {
            if (!ACS_Globals.IsPlayerPlaying) return;
            secondsSinceStart += Time.deltaTime;
            Timer.text = ACS_Utils.FormatTime(secondsSinceStart);
        }

        private void ClearScene()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery asteroidEntities = entityManager.CreateEntityQuery(typeof(ACS_AsteroidData), typeof(Collider));
            entityManager.DestroyEntity(asteroidEntities);
            EntityQuery powerUpEntities = entityManager.CreateEntityQuery(typeof(ACS_PowerUpData), typeof(Collider));
            entityManager.DestroyEntity(powerUpEntities);
            EntityQuery particleSystemRendererEntities = entityManager.CreateEntityQuery(typeof(ParticleSystemRenderer));
            entityManager.DestroyEntity(particleSystemRendererEntities);
            EntityQuery enemiesEntities = entityManager.CreateEntityQuery(typeof(ACS_EnemyData), typeof(Collider));
            entityManager.DestroyEntity(enemiesEntities);
            ScriptBehaviourUpdateOrder.RemoveWorldFromCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
            DefaultWorldInitialization.Initialize("Default World", false);
            ACS_Globals.IsPlayerPlaying = false;
            ACS_Globals.SpawnedLargeAsteroids = 0;
        }

        #endregion
    }
}
