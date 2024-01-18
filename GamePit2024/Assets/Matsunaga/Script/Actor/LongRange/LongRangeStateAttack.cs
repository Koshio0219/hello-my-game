using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Data;
using Game.Manager;
using KanKikuchi.AudioManager;

public class LongRangeStateAttack : MonoBehaviour, IPlayerState
{
    private GameObject _Bullet;
    private LongRangeStateController _longRangeStateController;
    private GameObject _BulletInstance;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _mainCamera;
    private Transform _player;
    private int _instanceID;
    private int _GamePadNumber;
    private float _attackPower;
    private bool isState;
    private string _anim_name = "Attack";

    public LongRangeStateAttack(GameObject Bullet, Animator animator, int GamePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, int InstanceID, float AttackPower)
    {
        _Bullet = Bullet;
        _GamePadNumber = GamePadNumber;
        _animator = animator;
        _rigidbody = rigidbody;
        _mainCamera = mainCamera;
        _player = Player;
        isState = false;
        _instanceID = InstanceID;
        _attackPower = AttackPower;
    }

    public PlayerState stayUpdate()
    {
        _animator.SetTrigger("AttackTrigger");
        SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT_CHARGE);
        SEManager.Instance.Play(SEPath.LONG_RANGE_SHOT);
        _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID, _attackPower);
        isState = true;

        if (isState)
        {
            isState = false;
            return PlayerState.IDLE;
        }
        return PlayerState.ATTACK;
    }

    public void enter()
    {
        _animator.SetTrigger("ReadyAttackTrigger");
        SEManager.Instance.Play(SEPath.LONG_RANGE_SHOT_CHARGE, isLoop: true);
        Vector3 playerPos = new Vector3(_player.position.x, _player.position.y + 0.5f, _player.position.z);
        Vector3 enemyPos = GameManager.stageManager.FindCloseEnemy(playerPos).gameObject.transform.position;
        if (enemyPos == null)
        {
            enemyPos = _player.forward;
        }
        //enemyPos.Normalize();
        var offse = (enemyPos - playerPos).normalized;
        _player.LookAt(new Vector3(enemyPos.x, _player.position.y, enemyPos.z));
        _BulletInstance = Instantiate<GameObject>(_Bullet, playerPos + offse * 0.8f, Quaternion.identity);
        _BulletInstance.GetComponent<BulletController>().setDirection(offse);

    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage() {
        SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT_CHARGE);
        SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT);
        _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID, _attackPower);
    }
    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }
}
