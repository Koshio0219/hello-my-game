using Game.Base;
using Game.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Data
{
    public enum BlockUseType
    {
        Normal,
        PlayerCreate
    }
    [System.Serializable]
    public struct BlockTypeUnit
    {
        public BlockUseType type;
        public BlockBase prefab;
    }

    [CreateAssetMenu(fileName = "BlockTypeConfig", menuName = "GamePit2024/BlockTypeConfig")]
    public class BlockTypeConfig : ScriptableObject
    {
        [SerializeField] private List<BlockTypeUnit> blockTypeUnits;

        private readonly Dictionary<BlockUseType, BlockTypeUnit> mapBlockTypeToUnit = new();
        public Dictionary<BlockUseType, BlockTypeUnit> MapBlockTypeToUnit
        {
            get
            {
                if(mapBlockTypeToUnit.Count == 0)
                {
                    foreach(var one in blockTypeUnits)
                    {
                        mapBlockTypeToUnit.AddOrSet(one.type, one);
                    }
                }
                return mapBlockTypeToUnit;
            }
        }

        public GameObject GetBlockPrefab(BlockUseType blockUseType)
        {
            if (MapBlockTypeToUnit.ContainsKey(blockUseType))
            {
                return mapBlockTypeToUnit[blockUseType].prefab.gameObject;
            }
            throw new System.Exception($"BlockTypeConfig not has the key of:{blockUseType},please check again!");
        }
    }
}