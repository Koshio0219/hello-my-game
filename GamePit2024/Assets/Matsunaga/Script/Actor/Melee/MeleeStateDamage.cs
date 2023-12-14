using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeStateDamage : IPlayerState
{
    private Animator _animator;
    private bool isState;
    private int _GamePadNumber;
    private string _anim_name = "Damage";

    public MeleeStateDamage(Animator animator, int GampePadNumber)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
    }

    public PlayerState stayUpdate()
    {
        if (isState) return PlayerState.IDLE;
        return PlayerState.DAMAGE;
    }

    public void enter()
    {
        _animator.animationStart(_anim_name);
        isState = false;
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    private void DamageEnd()
    {
        isState = true;
    }
}
