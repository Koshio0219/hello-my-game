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

        [Header("LongRange Actor(Basic)")]
        public int GamepadNumber_L;

        [Header("Director(Basic)")]
        public int GamepadNumber_D;
    }
}
