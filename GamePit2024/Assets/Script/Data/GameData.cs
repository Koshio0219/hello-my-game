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
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Game.Data
{
    public class GameData : Singleton<GameData>, IInit
    {
        public BlockTypeConfig BlockTypeConfig { get; private set; }
        public EnemyCreateConfig EnemyCreateConfig { get; private set; }
        public HudConfig HudConfig { get; private set; }
        public LevelConfig LevelConfig { get; private set; }
        private PlayerParameter playerParameter = null;

        public async void Init()
        {
            var token = Manager.GameManager.Instance.CancelTokenOnGameDestroy;
            BlockTypeConfig = await AssetLoader.Instance.Load<BlockTypeConfig>(AssetType.Config, "Assets/Config/BlockTypeConfig.asset", token);
            EnemyCreateConfig = await AssetLoader.Instance.Load<EnemyCreateConfig>(AssetType.Config, "Assets/Config/EnemyCreateConfig.asset", token);
            HudConfig = await AssetLoader.Instance.Load<HudConfig>(AssetType.Config, "Assets/Config/HudConfig.asset", token);
            LevelConfig = await AssetLoader.Instance.Load<LevelConfig>(AssetType.Config, "Assets/Config/LevelConfig.asset", token);
        }

        public async UniTask<PlayerParameter> GetPlayerParameter()
        {
            if (playerParameter == null)
            {
                playerParameter = await AssetLoader.Instance.Load<PlayerParameter>(AssetType.Config, "Assets/Config/PlayerParameter.asset", Manager.GameManager.Instance.CancelTokenOnGameDestroy);
            }
            return playerParameter;
        }
    }
}

