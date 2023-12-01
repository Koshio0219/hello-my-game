using Game.Base;
using Game.Framework;
using System.Collections;
using UnityEngine;

namespace Game.Unit
{
    [RequireComponent(typeof(Bullet))]
    public class BulletHit : MonoBehaviour
    {
        //public float damage;
        //public int sourceId;


        //private void OnTriggerEnter(Collider other)
        //{
        //    var one = other.gameObject.GetComponent<Enemy>();
        //    if (one != null)
        //    {
        //        EventQueueSystem.QueueEvent(new DamageEvent(sourceId,one.EnemyUnitData.InsId, damage));
        //    }
        //}

        public void OnEnterHit(Collider enter, int sourceId, float damage)
        {
            var up = enter.transform.GetRootParent();
            if (!up.TryGetComponent<IDamageable>(out var damageable)) return;
            //damageable.Hit(sourceId, damage);
            EventQueueSystem.QueueEvent(new SendDamageEvent(sourceId, up.gameObject.GetInstanceID(), damage));
        }
    }
}