using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.ACS.Scripts.Behaviours
{
    public class ACS_MenuUIManager : ACS_MonoBehaviour
    {

        public void StartGame()
        {
            SceneManager.LoadScene("ACS_Game");
        }

    }
}
