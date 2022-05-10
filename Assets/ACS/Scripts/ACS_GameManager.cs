using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Assets.ACS.Scripts
{
    public class ACS_GameManager : ACS_MonoBehaviour
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

        public int MaxAsteroidsOnScreen;
        public int Score;
        public int Lives;
        public Vector2 HorizontalEdges;
        public Vector2 VerticalEdges;

        public Entity[] LargeAsteroidPrefabs;
        public Entity[] MediumAsteroidPrefabs;
        public Entity[] SmallAsteroidPrefabs;

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
