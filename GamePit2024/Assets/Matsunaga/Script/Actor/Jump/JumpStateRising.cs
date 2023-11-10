using UnityEngine;
using DG.Tweening;

namespace animJump
{
    class JumpStateRising : IJumpState
    {
        private Animator _animator;
        private JumpData _jump_data;
        private JumpDistance _jump_distance;
        private Rigidbody _rigid_body;
        private bool _allow_add_force = true;
        private string _anim_name = "Rising";

        public JumpStateRising(Animator animator, JumpData jump_data,
                JumpDistance jump_distance, Rigidbody rigid_body)
        {
            _animator = animator;
            _jump_data = jump_data;
            _jump_distance = jump_distance;
            _rigid_body = rigid_body;
        }

        public JumpState stay_update()
        {
            if (_animator.isName(_anim_name) && _jump_distance.isFalling())
            {
                _allow_add_force = true;
                _jump_data.power_reset();
                return JumpState.FALLING;
            }

            return JumpState.RISING;
        }

        public void enter()
        {
            _animator.animationStart(_anim_name);
        }

        public void stay_fixed_update()
        {
            if (!_allow_add_force) return;
            _allow_add_force = false;
            DOTweenModulePhysics.DOMoveY(_rigid_body, _jump_data.power, 1.0f);
            //DOTweenModulePhysics.DOJump(_rigid_body, Vector3.up * _jump_data.power, _jump_data.power, 1, 1.0f);
            //_rigid_body.AddForce(Vector3.up * _jump_data.power, ForceMode.Impulse);
        }

        public void exit() { }
    }
}
