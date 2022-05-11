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
        }

        public void ShowGameEndScreen(string score)
        {
            EndGamePanel.gameObject.SetActive(true);
            Score.text = score;
        }

        public void Restart()
        {
            World.DisposeAllWorlds();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

    }
}
