using UnityEngine;

namespace animJump
{
    class JumpStateFalling : IJumpState
    {
        private Animator _animator;
        private JumpDistance _jump_distance;
        private string _anim_name = "Falling";
        private bool _Value = false;
        public bool _isPressed => false;
        public JumpStateFalling(Animator animator, JumpDistance jump_distance)
        {
            _animator = animator;
            _jump_distance = jump_distance;
        }

        public JumpState stay_update()
        {
            if (_animator.isName(_anim_name) && _jump_distance.isLanding())
            {
                return JumpState.LANDING;
            }

            return JumpState.FALLING;
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
