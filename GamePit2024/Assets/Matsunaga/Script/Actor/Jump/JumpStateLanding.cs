using UnityEngine;

namespace animJump
{
    class JumpStateLanding : IJumpState
    {
        private Animator _animator;
        private string _anim_name = "Landing";

        public JumpStateLanding(Animator animator)
        {
            _animator = animator;
        }

        public JumpState stay_update()
        {
            if (_animator.animationEnd(_anim_name))
            {
                return JumpState.IDLE;
            }

            return JumpState.LANDING;
        }

        public void enter()
        {
            _animator.animationStart(_anim_name);
        }

        public void stay_fixed_update() { }
        public void exit() {}

    }
}
