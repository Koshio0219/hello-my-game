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
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "GetHit01_SwordAndShiled")
        {
            return PlayerState.IDLE;
        }
        return PlayerState.DAMAGE;
    }

    public void enter()
    {
        _animator.SetTrigger("DamageTrigger");
        //_animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage() { }
}
