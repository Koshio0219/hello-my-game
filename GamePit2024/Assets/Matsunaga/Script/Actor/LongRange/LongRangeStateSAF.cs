using Game.Base;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KanKikuchi.AudioManager;

public class LongRangeStateSAF : MonoBehaviour, IPlayerState
{
    private GameObject _Bullet;
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
    private float _atk;

    public LongRangeStateSAF(GameObject Bullet, Animator animator, int GamePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, int InstanceID, float AttackPower,float atk)
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
        _atk = atk;
    }

    public PlayerState stayUpdate()
    {
        if (!Gamepad.all[_GamePadNumber].buttonNorth.isPressed)
        {
            _animator.SetTrigger("AttackTrigger");
            SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT_CHARGE);
            SEManager.Instance.Play(SEPath.LONG_RANGE_SHOT);
            _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID,_atk);
            isState = true;
        }

        if (isState)
        {
            isState = false;
            return PlayerState.IDLE;
        }
        return PlayerState.SKILLATTACKFIRST;
    }

    public void enter()
    {
        Vector3 _SetPosition = new Vector3(_player.position.x, _player.position.y + 1.0f, _player.position.z);
        Vector3 direction = GameManager.stageManager.FindCloseEnemy(_SetPosition).gameObject.transform.position;
        if (direction == null)
        {
            direction = _player.forward;
        }
        _animator.SetTrigger("ReadyAttackTrigger");
        SEManager.Instance.Play(SEPath.LONG_RANGE_SHOT_CHARGE, isLoop: true);

        //direction.Normalize();
        var offse = (direction - _SetPosition).normalized;
        _player.LookAt(new Vector3(direction.x, _player.position.y, direction.z));
        _BulletInstance = Instantiate<GameObject>(_Bullet, _SetPosition + offse * 0.8f, Quaternion.identity);
        _BulletInstance.GetComponent<BulletController>().setDirection(offse);

    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage()
    {
        SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT_CHARGE);
        SEManager.Instance.Stop(SEPath.LONG_RANGE_SHOT);
        _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID, _atk);
    }
    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }
}
