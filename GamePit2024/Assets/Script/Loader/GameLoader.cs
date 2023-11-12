using Game.Base;
using Game.Data;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Loader
{
    public class GameLoader : MonoBehaviour, IInit
    {
        [SerializeField]
        private DebugType debugType = DebugType.ALL;

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            Debug.DebugType = debugType;

            GameManager.Instance.Init();
            GameData.Instance.Init();
            //throw new System.NotImplementedException();
        }
    }
}

