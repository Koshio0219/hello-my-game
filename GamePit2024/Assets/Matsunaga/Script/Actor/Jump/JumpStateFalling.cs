using UnityEngine;
using UnityEngine.InputSystem;

namespace animJump
{
    class JumpStateFalling : IJumpState
    {
        private Animator _animator;
        private JumpDistance _jump_distance;
        private string _anim_name = "Falling";
        private int GamepadNumber = 0;
        private Camera _mainCamera;
        private Rigidbody _rigid_body;

        public JumpStateFalling(Animator animator, JumpDistance jump_distance, Rigidbody _rigidbody)
        {
            _animator = animator;
            _rigid_body = _rigidbody;
            _mainCamera = Camera.main;
            _jump_distance = jump_distance;
        }

        public JumpState stay_update()
        {
            if (_animator.isName(_anim_name) && _jump_distance.isLanding())
            {
                return JumpState.LANDING;
            }
            Move();
            return JumpState.FALLING;
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
                _rigid_body.MovePosition(_rigid_body.position + _velocity * 0.85f * Time.deltaTime);
            }
        }
    }
}
