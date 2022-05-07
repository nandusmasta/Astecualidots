using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ACS.Scripts
{
    public class ACS_GameManager : ACS_Monobehaviour
    {

        #region Properties

        private static ACS_GameManager _Instance;
        public static ACS_GameManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        #region Variables

        public int score;
        public int lives;
        public Vector2 horizontalEdges;
        public Vector2 verticalEdges;

        #endregion

        #region U3DLS

        public void Awake()
        {
            if (!_Instance)
            {
                _Instance = this;
            }
        }

        #endregion

    }
}
