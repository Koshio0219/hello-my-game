using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EnemyCreateConfig", menuName = "GamePit2024/EnemyCreateConfig")]
    public class EnemyCreateConfig : ScriptableObject
    {
        public List<EnemyUnitData> EnemyConfig;
    }
}
