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
    private bool isState;
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
        if (isState) return PlayerState.IDLE;
        return PlayerState.ATTACK;
    }

    public void enter()
    {
        _animator.SetTrigger("AttackTrigger");
        isState = false;
    }

    public void stayFixedUpdate() { }
    public void exit() { }
    private void AttackStart()
    {
        //eg
        GameHelper.ShootRay(_player.position, _player.forward, 10f, "Enemy", (info) =>
        {
            var up = info.transform.GetRootParent();
            var enemy = up.GetComponent<IEnemyBaseAction>();
            if (enemy != null)
            {
                var id = enemy.EnemyUnitData.InsId;
                Attack(id, _attackPower);
            }
        });
    }

    private void AttackEnd()
    {
        isState = true;
    }

    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }
}
