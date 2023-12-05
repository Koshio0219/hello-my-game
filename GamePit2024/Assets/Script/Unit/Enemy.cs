﻿using Cysharp.Threading.Tasks;
using Game.Action;
using Game.Base;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Game.Manager;
using Game.Framework;
using System;

namespace Game.Unit
{
    public class Enemy : MonoBehaviour, IEnemyBaseAction,IInit
    {
        private EnemyUnitData enemyUnitData;
        public EnemyUnitData EnemyUnitData => enemyUnitData;

        private EnemyState state;
        private EnemyState State { get => state; set => state = value; }
        public EnemyState EnemyState => State;

        protected readonly Dictionary<EnemyState, UnityAction> mapStateToAction =new(5);

        private AttackState attackState;
        private AttackState AttackState { get => attackState; set => attackState = value; }
        public AttackState EnemyAttackState => AttackState;

        private float maxHp; 
        public virtual float MaxHp
        {
            get
            {
                return maxHp;
            }
            set
            {
                maxHp = value;
            }
        }

        private float hp;
        public virtual float Hp
        {
            get
            {
                return hp;
            }
            set
            {
                if (hp == value) return;
                var last = value;
                hp = value;
                if (hp > MaxHp) hp = MaxHp;

                EventQueueSystem.QueueEvent(new EnemyHpChangeEvent(EnemyUnitData.InsId, last, hp));
            }
        }

        private float atk;
        public virtual float Atk
        {
            get
            {
                return atk;
            }
            set
            {
                atk = value;
            }
        }

        //private AsyncReactiveProperty<float> attack;
        //private AsyncReactiveProperty<float> maxHp;
        //private AsyncReactiveProperty<float> hp;

        [SerializeField] protected Animator animator;

        public virtual void Attack(int targetId, float damage)
        {
            ChangeState(EnemyState.Attack);
            Debug.Log($"Attacking! targetId:{targetId},damage:{damage}");
        }

        public virtual void Born(EnemyUnitData data)
        {
            Init();
            data.Init();
            enemyUnitData = data;
            SetBaseProp(data.prop);
            GameManager.stageManager.AddOneEnemy(data.InsId, this);
            ChangeState(EnemyState.Idle);

            EventQueueSystem.AddListener<SendDamageEvent>(DamageEventHandler);
        }

        public virtual void Dead()
        {
            EventQueueSystem.RemoveListener<SendDamageEvent>(DamageEventHandler);
            ChangeState(EnemyState.Dead);
        }

        protected virtual void DamageEventHandler(SendDamageEvent e)
        {
            if (e.damageActonType == DamageActonType.Trigger && e.enterObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            if (e.damageActonType == DamageActonType.PointTo && e.targetId != enemyUnitData.InsId) return;
            if (e.damageActonType == DamageActonType.Range && !e.rangeObjs.Contains(gameObject)) return;
            Hit(e.sourceId, e.damage);
        }

        public virtual void Hit(int sourceId, float damage)
        {
            if (sourceId == enemyUnitData.InsId) return;
            if (GameManager.stageManager.IsFriend(sourceId, enemyUnitData.InsId)) return;
            Hp -= damage;
            Debug.Log($"enemy id :{enemyUnitData.InsId},name:{gameObject.name} had receive damage:{damage},current hp :{Hp}");
            ChangeState(EnemyState.Hit);
        }

        public virtual void Move()
        {
            ChangeState(EnemyState.Moving);
        }

        protected virtual void SetBaseProp(EnemyBaseProp baseProp)
        {
            MaxHp = baseProp.maxHp;
            Hp = baseProp.Hp;
            Atk = baseProp.attack;
        }

        public virtual void ChangeState(EnemyState toState)
        {
            if (State == toState) return;
            State = toState;
            if (mapStateToAction.Count == 0) return;
            mapStateToAction[toState].Invoke();
        }

        public virtual void ChangeAttackState(AttackState toState)
        {
            if (AttackState == toState) return;
            AttackState = toState;
            OnChangeAttackState(toState);
        }

        protected virtual void OnChangeIdle() { }
        protected virtual void OnChangeDead() { }
        protected virtual void OnChangeHit() { }
        protected virtual void OnChangeMove() { }
        protected virtual void OnChangeAttack() { }
        protected virtual void OnChangeAttackState(AttackState attackState) { }

        public void Init()
        {
            if (mapStateToAction.Count > 0) return;
            mapStateToAction.Add(EnemyState.Idle, OnChangeIdle);
            mapStateToAction.Add(EnemyState.Dead, OnChangeDead);
            mapStateToAction.Add(EnemyState.Hit, OnChangeHit);
            mapStateToAction.Add(EnemyState.Moving, OnChangeMove);
            mapStateToAction.Add(EnemyState.Attack, OnChangeAttack);
        }
    }
}