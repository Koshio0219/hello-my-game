using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Framework;
using Game.Loader;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Game.Data
{
    public class GameData : Singleton<GameData>,IInit
    {
        public BlockTypeConfig BlockTypeConfig { get; private set; }

        public async void Init()
        {
            var token = GameManager.Instance.CancelTokenOnGameDestroy;
            BlockTypeConfig = await AssetLoader.Instance.Load<BlockTypeConfig>(AssetType.Config, "Assets/Config/BlockTypeConfig.asset", token);
        }
    }
}

