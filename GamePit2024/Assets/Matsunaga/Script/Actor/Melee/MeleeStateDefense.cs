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
    private GameObject field;

    public MeleeStateDefense(Animator animator, int GampePadNumber, GameObject deField)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
        field = deField;
    }

    public PlayerState stayUpdate()
    {
        if (Gamepad.all[_GamePadNumber].leftTrigger.isPressed) return PlayerState.DEFENSE;
        _animator.SetTrigger("ExitDefenseTrigger");
        field.SetActive(false);
        return PlayerState.IDLE;
        
    }

    public void enter()
    {
        _animator.SetTrigger("DefenseTrigger");
        field.SetActive(true);
        //_animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage(){}
}
