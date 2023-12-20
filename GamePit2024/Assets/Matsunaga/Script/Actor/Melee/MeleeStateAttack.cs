using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeStateAttack : IPlayerState
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _mainCamera;
    private Transform _player;
    private int _instanceID;
    private int _GamePadNumber;
    private float _attackPower;
    private string _anim_name = "Attack";

    public MeleeStateAttack(Animator animator, int GampePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, int InstanceID, float AttackPower)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
        _rigidbody = rigidbody;
        _mainCamera = mainCamera;
        _player = Player;
        _instanceID = InstanceID;
        _attackPower = AttackPower;
    }

    public PlayerState stayUpdate()
    {
        //Debug.Log(_animator.runtimeAnimatorController.animationClips[0].name);
        //Debug.Log(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (_animator.animationEnd(_anim_name))
        {
            return PlayerState.IDLE;
        }
        return PlayerState.ATTACK;
    }

    public void enter()
    {

        _animator.animationStart(_anim_name);
        //_animator.SetTrigger("AttackTrigger");
        /*RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
        UnityEditor.Animations.AnimatorController acc = ac as UnityEditor.Animations.AnimatorController;
        for (int layer = 0; layer < acc.layers.Length; layer++)
        {
            for (int s = 0; s < acc.layers[layer].stateMachine.states.Length; s++)
            {
                UnityEditor.Animations.ChildAnimatorState state = acc.layers[layer].stateMachine.states[s];
                Debug.Log(state.state.name);
            }
        }*/
        //Debug.Log("IsAnimationAttack: " + _animator.GetCurrentAnimatorStateInfo(0).IsName(_anim_name));
        /*GameHelper.ShootRay(_player.position, _player.forward, 10f, "Enemy", (info) =>
        {
            var up = info.transform.GetRootParent();
            var enemy = up.GetComponent<IEnemyBaseAction>();
            if (enemy != null)
            {
                var id = enemy.EnemyUnitData.InsId;
                Attack(id, _attackPower);
            }
        });*/
    }

    public void stayFixedUpdate() { }
    public void exit() { }

    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }
}
