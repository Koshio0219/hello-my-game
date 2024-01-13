using Game.Base;
using Game.Framework;
using Game.Test;
using System;
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
    private float _atk;

    public MeleeStateAttack(Animator animator, int GampePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, int InstanceID, float AttackPower,float atk)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
        _rigidbody = rigidbody;
        _mainCamera = mainCamera;
        _player = Player;
        _instanceID = InstanceID;
        _attackPower = AttackPower;
        _atk = atk;
    }

    public PlayerState stayUpdate()
    {
        //Debug.Log(_animator.runtimeAnimatorController.animationClips[0].name);
        //Debug.Log(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack01_SwordAndShiled")
        {
            return PlayerState.IDLE;
        }
        return PlayerState.ATTACK;
    }

    public void enter()
    {

        _animator.animationStart(_anim_name);
        _animator.SetTrigger("AttackTrigger");
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
        Debug.Log("IsAnimationAttack: " + _animator.GetCurrentAnimatorStateInfo(0).IsName(_anim_name));
        //eg (attack range is 2)
        var pos = _player.position.FixHeight(_player.position.y+ 0.75f);
        /*GameHelper.ShootRay(pos, _player.forward, 2f, "", (info) =>
        {
            var up = info.transform.GetRootParent();
            if (up.TryGetComponent<IEnemyBaseAction>(out var enemy))
            {
                var id = enemy.EnemyUnitData.InsId;
                Attack(id, _atk);
            }
        });*/

        ShootBoxRay(pos, Vector3.one * 0.8f, _player.forward, 1.2f, "", (info) =>
        {
            var up = info.transform.GetRootParent();
            if (up.TryGetComponent<IEnemyBaseAction>(out var enemy))
            {
                var id = enemy.EnemyUnitData.InsId;
                Attack(id, _atk);
            }
        });
    }

    public void stayFixedUpdate() { }
    public void exit() { }

    private void Attack(int targetID, float damage)
    {
        //...
        Debug.Log($"Player Attack !! target Id: {targetID}");
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }

    public static bool ShootBoxRay(Vector3 orgin, Vector3 boxSize, Vector3 dir, float dis, string tag, Action<RaycastHit> callback)
    {

        //Debug.DrawLine(orgin, orgin + dir * dis, UnityEngine.Color.red);
        if (Physics.BoxCast(orgin, boxSize, dir, out RaycastHit info, Quaternion.identity, dis))
        {
            if (tag == "" || info.collider.CompareTag(tag))
            {
                callback?.Invoke(info);
                return true;
            }
        }
        return false;
    }
}
