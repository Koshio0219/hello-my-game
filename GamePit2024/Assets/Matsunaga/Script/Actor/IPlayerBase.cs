using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Manager;
using Game.Framework;
using System;

namespace Game.Base
{
    interface IPlayerBase : IDamageable
    {
        void Dead();
        void Attack(int targetID, float damage);
        void Move();
        //void Hit(int sourceId, float damage);
    }

    public class Player :MonoBehaviour, IPlayerBase
    {
        public int InsId
        {
            get => gameObject.GetInstanceID();
        }

        protected virtual float Hp { get; set; }
        //public float Atk;

        protected virtual void Awake() 
        {
            GameManager.stageManager.AddOnePlayer(InsId, gameObject);
            EventQueueSystem.AddListener<SendDamageEvent>(DamageEventHandler);
        }

        private void DamageEventHandler(SendDamageEvent e)
        {
            if (e.damageActonType == DamageActonType.Trigger && e.enterObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            if (e.damageActonType == DamageActonType.PointTo && e.targetId != InsId) return;
            if (e.damageActonType == DamageActonType.Range && !e.rangeObjs.Contains(gameObject)) return;
            Hit(e.sourceId, e.damage);
        }

        public virtual void Attack(int targetID, float damage)
        {

        }

        public virtual void Dead()
        {
            GameManager.stageManager.RemoveOnePlayer(InsId, gameObject);
            EventQueueSystem.RemoveListener<SendDamageEvent>(DamageEventHandler);
        }

        public virtual void Hit(int sourceId, float damage)
        {
            if (sourceId == InsId) return;
            if (GameManager.stageManager.IsFriend(sourceId, InsId)) return;
            Debug.Log($"player id :{InsId},name:{gameObject.name} had receive damage:{damage} from id:{sourceId}");
            //今、Hp は base class　に　い　ない　
            Hp -= damage;
        }

        public virtual void Move()
        {

        }
    }
}
