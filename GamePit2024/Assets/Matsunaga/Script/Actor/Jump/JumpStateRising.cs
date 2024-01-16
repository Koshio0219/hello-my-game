using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

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
        private int GamepadNumber = 0;
        private Camera _mainCamera;

        public JumpStateRising(Animator animator, JumpData jump_data,
                JumpDistance jump_distance, Rigidbody rigid_body)
        {
            _animator = animator;
            _jump_data = jump_data;
            _jump_distance = jump_distance;
            _rigid_body = rigid_body;
            _mainCamera = Camera.main;
        }

        public JumpState stay_update()
        {
            if (_animator.isName(_anim_name) && _jump_distance.isFalling())
            {
                _allow_add_force = true;
                _jump_data.power_reset();
                return JumpState.FALLING;
            }

            Move();

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

      　　　//TODO when jumpping,press left or right can move at sky
            //DOTweenModulePhysics.DOJump(_rigid_body, Vector3.up * _jump_data.power, _jump_data.power, 1, 1.0f);
            //_rigid_body.AddForce(Vector3.up * _jump_data.power, ForceMode.Impulse);
        }

        public void exit() { }

        public void SetGamepadNumber(int _GamepadNumber)
        {
            GamepadNumber = _GamepadNumber;
        }

        private void Move()
        {
            Vector3 _cameraForward = Vector3.Scale(_mainCamera.gameObject.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 _vertical = _cameraForward * Gamepad.all[GamepadNumber].leftStick.ReadValue().y;
            Vector3 _horizontal = _mainCamera.gameObject.transform.right * Gamepad.all[GamepadNumber].leftStick.ReadValue().x;
            Vector3 _moveForward = _vertical + _horizontal;
            float _moveSpeed = Mathf.LerpUnclamped(0f, 5f, 0.85f);
            Vector3 _velocity = _moveForward.normalized * _moveSpeed;
            if (_velocity.magnitude > 0.085f)
            {

                _jump_distance.GetSelfTransform().LookAt(_jump_distance.GetSelfTransform().position + _velocity);
                _rigid_body.MovePosition(_rigid_body.position + _velocity * 0.85f* Time.deltaTime);
            }
        }
    }
}
