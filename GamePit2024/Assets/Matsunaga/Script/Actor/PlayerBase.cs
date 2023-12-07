using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Manager;

namespace Game.Base
{
    interface PlayerBase : IDamageable
    {
        void Dead();
        void Attack(int targetID, float damage);
        void Move();
        //void Hit(int sourceId, float damage);
    }

    public class Player :MonoBehaviour, PlayerBase
    {
        public int InsId
        {
            get => gameObject.GetInstanceID();
        }

        public float Atk;

        protected virtual void Awake() 
        {
            GameManager.stageManager.AddOnePlayer(InsId, gameObject);
        }

        public virtual void Attack(int targetID, float damage)
        {

        }

        public virtual void Dead()
        {

        }

        public virtual void Hit(int sourceId, float damage)
        {

        }

        public virtual void Move()
        {

        }
    }
}
