using Game.Base;
using Game.Framework;
using Game.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeStateDefense : IPlayerState
{
    private Animator _animator;
    private bool isState;
    private int _GamePadNumber;
    private string _anim_name = "Defense";

    public MeleeStateDefense(Animator animator, int GampePadNumber)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
    }

    public PlayerState stayUpdate()
    {
        if (Gamepad.all[_GamePadNumber].leftTrigger.isPressed) return PlayerState.DEFENSE;
        _animator.SetTrigger("ExitDefenseTrigger");
        return PlayerState.IDLE;
        
    }

    public void enter()
    {
        _animator.SetTrigger("DefenseTrigger");
        //_animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage(){}
}
