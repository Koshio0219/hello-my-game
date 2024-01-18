using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct LevelData
    {
        public int goalPoint;
        public int showTime;
        public Color backgroundColor;
    }

    [CreateAssetMenu(fileName = "LevelConfig", menuName = "GamePit2024/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelData> levelDatas;
        public GameObject goalPrefab;
        public GameObject gemPrefab;
    }
}
