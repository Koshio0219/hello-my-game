using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Framework;
using Game.Loader;
using Game.Manager;
using Game.Test;
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
        private PlayerParameter playerParameter =null;

        public async void Init()
        {
            var token = GameManager.Instance.CancelTokenOnGameDestroy;
            BlockTypeConfig = await AssetLoader.Instance.Load<BlockTypeConfig>(AssetType.Config, "Assets/Config/BlockTypeConfig.asset", token);
        }

        public async UniTask<PlayerParameter> GetPlayerParameter()
        {
            if (playerParameter == null)
            {
                playerParameter = await AssetLoader.Instance.Load<PlayerParameter>(AssetType.Config, "Assets/Config/PlayerParameter.asset", GameManager.Instance.CancelTokenOnGameDestroy);
            }
            return playerParameter;
        }
    }
}

