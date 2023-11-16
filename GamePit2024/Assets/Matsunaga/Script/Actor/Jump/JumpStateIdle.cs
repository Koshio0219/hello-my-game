﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace animJump
{
    class JumpStateIdle : IJumpState
    {
        private Animator _animator;
        private string _anim_name = "Idle";
        private int GamepadNumber = 0;

        public JumpStateIdle(Animator animator)
        {
            _animator = animator;
        }

        public JumpState stay_update()
        {
            if ((_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || _animator.isName(_anim_name)) && Gamepad.all[GamepadNumber].buttonSouth.isPressed)
            {
                return JumpState.WAITING;
            }

            return JumpState.IDLE;
        }

        public void enter()
        {
            _animator.animationStart(_anim_name);
        }

        public void stay_fixed_update() { }
        public void exit() { }
        public void SetGamepadNumber(int _GamepadNumber)
        {
            GamepadNumber = _GamepadNumber;
        }
    }
}
