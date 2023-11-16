using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Test
{
    [CreateAssetMenu(fileName = "PlayerParameter", menuName = "PlayerParameter")]
    public class PlayerParameter : Game.Framework.MonoSingleton<PlayerParameter>
    {
        [Header("Melee Actor(Basic)")]
        public int GamepadNumber_M;

        [Header("LongRange Actor(Basic)")]
        public int GamepadNumber_L;

        [Header("Director(Basic)")]
        public int GamepadNumber_D;
    }
}
