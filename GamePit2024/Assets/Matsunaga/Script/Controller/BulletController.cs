using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifeSpan = 5.0f;
    private Vector3 direction;
    private bool AttackTrigger;
    private int sourceId;
    private float damage;
    private float originScale;
    // Start is called before the first frame update
    void Start()
    {
        originScale = this.transform.localScale.x;
        AttackTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackTrigger)
        {
            BulletMove();
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0f)
            {
                Debug.Log("LifeOVer");
                Destroy(gameObject);
            }
        }
        else
        {
            if (this.transform.localScale.x < 1.5f)
            {
                this.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f) * Time.deltaTime;
            }
        }

    }
    public void setAttackTrigger(int id,float _damage = 10f)
    {
        AttackTrigger = true;
        sourceId = id;
        damage = _damage;
    }

    public void setDirection(Vector3 Direction)
    {
        direction = Direction;
    }

    private void BulletMove() {
        this.transform.position = this.transform.position + direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!AttackTrigger) return;
        var up = other.transform.GetRootParent();
        float rate = this.transform.localScale.x / originScale;
        if (!up.TryGetComponent<IDamageable>(out _)) return;
        if (up.TryGetComponent<Player>(out _)) return;
        EventQueueSystem.QueueEvent(new SendDamageEvent(sourceId, up.gameObject, Mathf.Floor(damage + rate * rate)));
        Debug.Log("Attack to " + up.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!AttackTrigger) return;
        var up = other.transform.GetRootParent();
        float rate = this.transform.localScale.x / originScale;
        if (!up.TryGetComponent<IDamageable>(out _)) return;
        if (up.TryGetComponent<Player>(out _)) return;
        EventQueueSystem.QueueEvent(new SendDamageEvent(sourceId, up.gameObject, Mathf.Floor(damage + rate * rate)));
        Debug.Log("Attack to " + up.gameObject);
        Destroy(gameObject);
    }
}
