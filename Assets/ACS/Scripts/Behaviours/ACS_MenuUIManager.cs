/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

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
