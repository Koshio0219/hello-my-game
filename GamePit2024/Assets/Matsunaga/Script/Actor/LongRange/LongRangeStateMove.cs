using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LongRangeStateMove : IPlayerState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _mainCamera;
    private Transform _player;
    private int _GamePadNumber;
    private string _anim_name = "Walk";
    private float _speed;

    public LongRangeStateMove(Animator animator, int GampePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, float speed)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
        _rigidbody = rigidbody;
        _mainCamera = mainCamera;
        _player = Player;
        _speed = speed;

    }

    public PlayerState stayUpdate()
    {
        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonEast.wasPressedThisFrame)
        {
            _animator.SetFloat("Speed", 0f);
            return PlayerState.ATTACK;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonSouth.isPressed)
        {
            _animator.SetFloat("Speed", 0f);
            return PlayerState.JUMP;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].leftStick.ReadValue().magnitude <= 0.085f)
        {
            _animator.SetFloat("Speed", 0f);
            return PlayerState.IDLE;
        }
        else
        {
            Move();
        }

        return PlayerState.MOVE;
    }

    public void enter()
    {
        _animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    private void Move()
    {
        Vector3 _cameraForward = Vector3.Scale(_mainCamera.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 _vertical = _cameraForward * Gamepad.all[_GamePadNumber].leftStick.ReadValue().y;
        Vector3 _horizontal = _mainCamera.right * Gamepad.all[_GamePadNumber].leftStick.ReadValue().x;
        Vector3 _moveForward = _vertical + _horizontal;
        float _moveSpeed = Mathf.LerpUnclamped(0f, 5f, _speed);
        Vector3 _velocity = _moveForward.normalized * _moveSpeed;
        if (_velocity.magnitude > 0.085f)
        {
            _animator.SetFloat("Speed", _velocity.magnitude);

            _player.LookAt(_player.position + _velocity);
            _rigidbody.MovePosition(_rigidbody.position + _velocity * _speed * Time.deltaTime);
        }
    }
}
