using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LongRangeStateSAS : MonoBehaviour, IPlayerState
{
    private GameObject _Beam;
    private List<GameObject> _BeamInstance;
    private int BeamLoad;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _mainCamera;
    private Transform _player;
    private int _instanceID;
    private int _GamePadNumber;
    private float _attackPower;
    private bool isState;
    private string _anim_name = "Attack";

    public LongRangeStateSAS(GameObject Beam, Animator animator, int GamePadNumber, Rigidbody rigidbody, Transform mainCamera, Transform Player, int InstanceID, float AttackPower)
    {
        _Beam = Beam;
        _GamePadNumber = GamePadNumber;
        _animator = animator;
        _rigidbody = rigidbody;
        _mainCamera = mainCamera;
        _player = Player;
        isState = false;
        _instanceID = InstanceID;
        _attackPower = AttackPower;
        
        BeamLoad = 4;
        _BeamInstance = new List<GameObject>();

    }

    public PlayerState stayUpdate()
    {
        if (!Gamepad.all[_GamePadNumber].buttonWest.isPressed)
        {
            _animator.SetTrigger("AttackTrigger");
            for(int i = 0; i < _BeamInstance.Count; i++)
            {
                _BeamInstance[i].GetComponent<BeamController>().setAttackTrigger();
            }
           
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
        return PlayerState.SKILLATTACKSECOND;
    }

    public void enter()
    {
        _animator.SetTrigger("ReadyAttackTrigger");
        Vector3 _SetPosition = new Vector3(_player.position.x, 1.5f, _player.position.z);
        Vector3 direction = _player.forward;
        direction.Normalize();

        _BeamInstance.Clear();

        for(int i = 0; i < BeamLoad; i++)
        {
            _BeamInstance.Add(Instantiate<GameObject>(_Beam, Vector3.zero, Quaternion.identity));
        }
        for(int i = 0; i < BeamLoad; i++)
        {
            float t = i * 2.0f * Mathf.PI / BeamLoad;
            float x = Mathf.Sin(t) * 2.0f;
            float z = Mathf.Cos(t) * 2.0f;
            _BeamInstance[i].transform.position = _SetPosition + new Vector3(x, 0.0f, z);
            //_BeamInstance[i] = Instantiate<GameObject>(_Beam, _SetPosition + new Vector3(x, 0.0f, z), Quaternion.identity);
            _BeamInstance[i].GetComponent<BeamController>().Init(_SetPosition, _SetPosition + direction * 4f, 2.0f, t);
        }
    }

    public void stayFixedUpdate() { }
    public void exit() { }

    private void Attack(int targetID, float damage)
    {
        //...
        EventQueueSystem.QueueEvent(new SendDamageEvent(_instanceID, targetID, damage));
    }

}
