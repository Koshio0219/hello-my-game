using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeStateDamage : IPlayerState
{
    private Animator _animator;
    private bool isState;
    private int _GamePadNumber;
    //private string _anim_name = "Damage";

    public LongRangeStateDamage(Animator animator, int GampePadNumber)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
    }

    public PlayerState stayUpdate()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Witch_Damage")
        {
            return PlayerState.IDLE;
        }
        return PlayerState.DAMAGE;
    }

    public void enter()
    {
        _animator.SetTrigger("DamageTrigger");
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage() { }
}

