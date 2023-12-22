using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class BeamController : MonoBehaviour
{
    private bool AttackTrigger;
    private int sourceId;
    private float damage;
    private Vector3 _velocity;
    private Vector3 _axis;
    private Vector3 _selfPosition;
    private Vector3 _targetPosition;
    private float _period;
    private float _time;
    private float _offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AttackTrigger)
        {
            BeamMove();
            _period -= Time.deltaTime;
            if (_period <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //if (_selfPosition.magnitude == 0) return;
            Debug.Log(_selfPosition);
            transform.position = _selfPosition + new Vector3(2.0f * Mathf.Cos(_time + _offset), 1.0f * Mathf.Sin(_time + _offset), 2.0f * Mathf.Sin(_time + _offset));
            _time += Time.deltaTime;
            //transform.RotateAround(_selfPosition, _axis, 360 / _period * Time.deltaTime + (180 * _offset) / 4.0f);
            /*var tr = transform;
            // 夞揮偺僋僅乕僞僯僆儞嶌惉
            var angleAxis = Quaternion.AngleAxis(60.0f * Time.deltaTime, _axis);

            // 墌塣摦偺埵抲寁嶼
            var pos = tr.position;

            pos -= _selfPosition;
            pos = angleAxis * pos;
            pos += _selfPosition;

            transform.position = pos;
            _velocity = (transform.position - _selfPosition).normalized * 3.0f;*/
        }
    }

    public void setAttackTrigger(int id, float _damage = 10f)
    {
        AttackTrigger = true;
        sourceId = id;
        damage = _damage;
    }

    public void setTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void Init(Vector3 selfPosition, Vector3 targetPosition, float Period, float Offset)
    {
        AttackTrigger = false;
        //_axis = new Vector3(0, 1, 0);
        _velocity = new Vector3(0, 2.0f, 0);
        _time = 0f;
        _selfPosition = selfPosition;
        _targetPosition = targetPosition;
        if(Period > 0f)
        {
            _period = Period;
        } else
        {
            _period = 5f;
        }
        _offset = Offset;
    }

    private void BeamMove()
    {
        Vector3 acceleration = Vector3.zero;
        Vector3 diff = _targetPosition - transform.position;
        acceleration += (diff - _velocity * _period) * 2f / (_period * _period);
        _velocity += acceleration * Time.deltaTime;
        this.transform.position = this.transform.position + _velocity * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        var up = other.transform.GetRootParent();
        if (!up.TryGetComponent<IDamageable>(out _)) return;
        EventQueueSystem.QueueEvent(new SendDamageEvent(sourceId, up.gameObject, damage));
        Destroy(gameObject);
    }
}
