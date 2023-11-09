using UnityEngine;
using UnityEngine.InputSystem;

namespace animJump
{
    class JumpStateWaiting : IJumpState
    {
        private Animator _animator;
        private JumpData _jump_data;
        private string _anim_name = "Waiting";

        public JumpStateWaiting(Animator animator, JumpData jump_data)
        {
            _animator = animator;
            _jump_data = jump_data;
        }

        public JumpState stay_update()
        {
            _jump_data.power_up();

            if (_animator.isName(_anim_name) && !Gamepad.current.buttonSouth.isPressed)
            {
                return JumpState.RISING;
            }

            return JumpState.WAITING;
        }

        public void enter()
        {
            _animator.animationStart(_anim_name);
        }

        public void stay_fixed_update() { }
        public void exit() { }
    }
}
