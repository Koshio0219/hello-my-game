using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    private bool AttackTrigger;

    private Vector3 _velocity;
    private Vector3 _axis;
    private Vector3 _selfPosition;
    private Vector3 _targetPosition;
    private float _period;
    private float _offset;

    private void OnTriggerEnter(Collider other)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        AttackTrigger = false;
        _selfPosition = Vector3.zero;
        _axis = new Vector3(0, 1, 0);
        _velocity = new Vector3(0, 1, 0);
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
            //transform.RotateAround(_selfPosition, _axis, 360 / _period * Time.deltaTime + (180 * _offset) / 4.0f);
            var tr = transform;
            // ‰ñ“]‚ÌƒNƒH[ƒ^ƒjƒIƒ“ì¬
            var angleAxis = Quaternion.AngleAxis(60.0f * Time.deltaTime, _axis);

            // ‰~‰^“®‚ÌˆÊ’uŒvŽZ
            var pos = tr.position;

            pos -= _selfPosition;
            pos = angleAxis * pos;
            pos += _selfPosition;

            transform.position = pos;
            _velocity = (transform.position - _selfPosition).normalized * 3.0f;
        }
    }

    public void setAttackTrigger()
    {
        AttackTrigger = true;
    }

    public void setTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void Init(Vector3 selfPosition, Vector3 targetPosition, float Period, float Offset)
    {
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
}
