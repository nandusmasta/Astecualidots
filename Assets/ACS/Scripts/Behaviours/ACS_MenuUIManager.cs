/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Behaviours
{
    using UnityEngine.SceneManagement;

    public class ACS_MenuUIManager : ACS_MonoBehaviour
    {
        #region Methods

        public void StartGame()
        {
            SceneManager.LoadScene("ACS_Game");
        }

        #endregion
    }
}
