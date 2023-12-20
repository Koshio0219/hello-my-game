using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LongRangeStateIdle : IPlayerState
{
    private Animator _animator;
    private int _GamePadNumber;
    private string _anim_name = "Idle";

    public LongRangeStateIdle(Animator animator, int GampePadNumber)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
    }

    public PlayerState stayUpdate()
    {
        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].leftStick.ReadValue().magnitude > 0.085f)
        {
            return PlayerState.MOVE;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonEast.wasPressedThisFrame)
        {
            return PlayerState.ATTACK;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonNorth.wasPressedThisFrame)
        {
            return PlayerState.SKILLATTACKFIRST;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonWest.wasPressedThisFrame)
        {
            return PlayerState.SKILLATTACKSECOND;
        }

        if (_animator.isName(_anim_name) && Gamepad.all[_GamePadNumber].buttonSouth.isPressed)
        {
            return PlayerState.JUMP;
        }

        return PlayerState.IDLE;
    }

    public void enter()
    {
        _animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }
}
