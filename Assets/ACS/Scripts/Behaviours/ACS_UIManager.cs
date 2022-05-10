using Assets.ACS.Scripts.Utils;
using UnityEngine.UI;

namespace Assets.ACS.Scripts.Behaviours
{
    public class ACS_UIManager : ACS_MonoBehaviour
    {

        public Text score;

        public void Update()
        {
            score.text = ACS_Globals.Score.ToString();
        }

    }
}
