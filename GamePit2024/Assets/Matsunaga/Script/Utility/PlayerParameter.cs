using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Test
{
    [CreateAssetMenu(fileName = "PlayerParameter", menuName = "GamePit2024/PlayerParameter")]
    public class PlayerParameter : ScriptableObject
    {
        [Header("Melee Actor(Basic)")]
        public int GamepadNumber_M;
        public float attack_M;
        public float hp_M;
        public GameObject prefab_M;

        [Header("LongRange Actor(Basic)")]
        public int GamepadNumber_L;
        public float attack_L;
        public float hp_L;
        public GameObject prefab_L;

        [Header("Director(Basic)")]
        public int GamepadNumber_D;
    }
}
