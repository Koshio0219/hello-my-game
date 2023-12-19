using Game.Base;
using Game.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public struct BlockUnitData
    {
        public BlockBaseType baseType;
        public BlockUseType useType;
        public int offseY;
    }

    [System.Serializable]
    public struct BlockColumnData
    {
        public List<BlockUnitData> blockUnits;
    }

    [CreateAssetMenu(fileName = "BlockCreateData", menuName = "GamePit2024/BlockCreateData")]
    public class BlockCreateData : ScriptableObject
    {
        public List<BlockColumnData> blockUnitDatas;
    }
}