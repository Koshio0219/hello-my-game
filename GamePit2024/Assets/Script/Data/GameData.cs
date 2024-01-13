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
    public enum CharacterType
    {
        Directer,
        Melee,
        LongRange,
    }

    public class GameData : Singleton<GameData>, IInit
    {
        public BlockTypeConfig BlockTypeConfig { get; private set; }
        public EnemyCreateConfig EnemyCreateConfig { get; private set; }
        public HudConfig HudConfig { get; private set; }
        public LevelConfig LevelConfig { get; private set; }
        public PlayerParameter PlayerParameter { get; set; }

        public List<CharacterType> SelectedCharacters { get; private set; } = new();

        public async void Init()
        {
            var token = Manager.GameManager.Instance.CancelTokenOnGameDestroy;
            BlockTypeConfig = BlockTypeConfig != null ? BlockTypeConfig : await AssetLoader.Instance.Load<BlockTypeConfig>(AssetType.Config, "Assets/Config/BlockTypeConfig.asset", token);
            EnemyCreateConfig = EnemyCreateConfig != null ? EnemyCreateConfig : await AssetLoader.Instance.Load<EnemyCreateConfig>(AssetType.Config, "Assets/Config/EnemyCreateConfig.asset", token);
            HudConfig = HudConfig != null ? HudConfig : await AssetLoader.Instance.Load<HudConfig>(AssetType.Config, "Assets/Config/HudConfig.asset", token);
            LevelConfig = LevelConfig != null ? LevelConfig : await AssetLoader.Instance.Load<LevelConfig>(AssetType.Config, "Assets/Config/LevelConfig.asset", token);
            PlayerParameter = PlayerParameter != null ? PlayerParameter : await AssetLoader.Instance.Load<PlayerParameter>(AssetType.Config, "Assets/Config/PlayerParameter.asset", token);
        }

        public void AddSelectCharacter(CharacterType type)
        {
            if (SelectedCharacters.Contains(type)) return;
            SelectedCharacters.Add(type);
        }

        public void ClearSelectCharacter() => SelectedCharacters.Clear();

        //test
        //public void Test()
        //{
        //PlayerParameter.GamepadNumber_M = 2;
        //}
    }
}

