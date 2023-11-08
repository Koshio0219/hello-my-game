using UnityEngine;
using UnityEngine.InputSystem;

namespace animJump
{
    class JumpStateIdle : IJumpState
    {
        private Animator _animator;
        private string _anim_name = "Idle";
        
        private bool _Value = false;
        public bool _isPressed => _Value;
        public JumpStateIdle(Animator animator)
        {
            _animator = animator;
        }

        public JumpState stay_update()
        {
            if ((_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || _animator.isName(_anim_name)) && (Gamepad.current.buttonSouth.isPressed || _Value))
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
        public void SetPressed(bool value)
        {
            _Value = value;
        }
    }
}
