using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (!Gamepad.all[_GamePadNumber].buttonEast.isPressed)
        {
            _animator.SetTrigger("AttackTrigger");
            _BulletInstance.GetComponent<BulletController>().setAttackTrigger(_instanceID);
            isState = true;
        }
        //Debug.Log(_animator.runtimeAnimatorController.animationClips[0].name);
        //Debug.Log(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //if (_animator.animationEnd(_anim_name))
        //{
        //    return PlayerState.IDLE;
        //}
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
        Vector3 _SetPosition = new Vector3(_player.position.x, 1.5f, _player.position.z);
        Vector3 direction = _player.forward;
        direction.Normalize();
        _BulletInstance = Instantiate<GameObject>(_Bullet, _SetPosition + direction * 1.2f, Quaternion.identity);
        _BulletInstance.GetComponent<BulletController>().setDirection(direction);
        //StartCoroutine(ChargeShot());
        //_animator.animationStart(_anim_name);
    }

    public void stayFixedUpdate() { }
    public void exit() { }

    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }

    IEnumerator ChargeShot()
    {//チャージショット

        Vector3 _SetPosition = new Vector3(_player.position.x, 1.5f, _player.position.z);
        Vector3 direction = _player.forward;
        direction.Normalize();
        //var bulletInstance = Instantiate<GameObject>(_Bullet, _SetPosition + direction * 1.2f, Quaternion.identity);
        //bulletInstance.GetComponent<BulletController>().setDirection(direction);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true);
        isState = true;
        //bulletInstance.GetComponent<BulletController>().setAttackTrigger();
        //PlayerState.IDLE;
    }
}
