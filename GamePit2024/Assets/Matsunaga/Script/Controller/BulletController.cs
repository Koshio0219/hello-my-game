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
    // Start is called before the first frame update
    void Start()
    {
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
                Destroy(gameObject);
            }
        }
        else
        {
            if (this.transform.localScale.x < 1.0)
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
        var up = other.transform.GetRootParent();
        if (!up.TryGetComponent<IDamageable>(out _)) return;
        EventQueueSystem.QueueEvent(new SendDamageEvent(sourceId, up.gameObject, damage));
        Destroy(gameObject);
    }
}
