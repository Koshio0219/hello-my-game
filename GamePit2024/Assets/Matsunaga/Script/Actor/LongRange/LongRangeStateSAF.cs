using Game.Base;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        _animator.SetTrigger("ReadyAttackTrigger");
        Vector3 _SetPosition = new Vector3(_player.position.x, _player.position.y + 1.0f, _player.position.z);
        Vector3 direction = GameManager.stageManager.FindCloseEnemy(_SetPosition).gameObject.transform.position;
        direction.Normalize();
        _player.LookAt(_SetPosition + direction);
        _BulletInstance = Instantiate<GameObject>(_Bullet, _SetPosition + direction * 0.8f, Quaternion.identity);
        _BulletInstance.GetComponent<BulletController>().setDirection(direction);

    }

    public void stayFixedUpdate() { }
    public void exit() { }
    public void enterDamage()
    {
        _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID, _atk);
    }
    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }
}
