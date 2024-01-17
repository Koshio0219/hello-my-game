using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Manager;
using Game.Framework;
using System;
using Game.Test;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Game.Base
{
    interface IPlayerBase : IDamageable
    {
        void Dead();
        void Attack(int targetID, float damage);
        void Move();
        //void Hit(int sourceId, float damage);
    }

    public class Player : MonoBehaviour, IPlayerBase, IEventListenerAction
    {
        [SerializeField]protected Rigidbody _rigidBody;
        private RigidbodyConstraints rigidbodyConstraints;
        public int InsId
        {
            get => gameObject.GetInstanceID();
        }

        protected PlayerParameter _PlayerParameter;

        public virtual float Hp { get; set; }
        public virtual PlayerType PlayerType { get; }
        //public float Atk;

        private void Awake()
        {
            AddListeners();
        }

        protected virtual void Start()
        {
            _PlayerParameter = GameData.Instance.PlayerParameter;
            GameManager.stageManager.AddOnePlayer(InsId, gameObject);
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
            GameManager.stageManager.RemoveOnePlayer(InsId);
            //RemoveListeners();

            //when one player dead,game over
            EventQueueSystem.QueueEvent(new PlayerDeadEvent());
        }

        public virtual void Hit(int sourceId, float damage)
        {
            if (sourceId == InsId) return;
            if (GameManager.stageManager.IsFriend(sourceId, InsId)) return;


            //.... ぼうぎょ処理

            Debug.Log($"player id :{InsId},name:{gameObject.name} had receive damage:{damage} from id:{sourceId}");
            //今、Hp は base class　に　い　ない　
            EventQueueSystem.QueueEvent(new PopupTextEvent(transform, (int)damage,Color.red));

            var lastHp = Hp;
            Hp -= damage;
            EventQueueSystem.QueueEvent(new PlayerHpChangeEvent(PlayerType, lastHp, Hp));

            if (Hp <= 0) Dead();
        }

        public virtual void Move()
        {

        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        public void AddListeners()
        {
            EventQueueSystem.AddListener<SendDamageEvent>(DamageEventHandler);
            EventQueueSystem.AddListener<EnterDeadZoneEvent>(EnterDeadZoneHandler);
            EventQueueSystem.AddListener<BlockDragStartEvent>(BlockDragStartHandler);
            EventQueueSystem.AddListener<BlockDragEndEvent>(BlockDragEndHandlerAsync);
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        public void RemoveListeners()
        {
            EventQueueSystem.RemoveListener<SendDamageEvent>(DamageEventHandler);
            EventQueueSystem.RemoveListener<EnterDeadZoneEvent>(EnterDeadZoneHandler);
            EventQueueSystem.RemoveListener<BlockDragStartEvent>(BlockDragStartHandler);
            EventQueueSystem.RemoveListener<BlockDragEndEvent>(BlockDragEndHandlerAsync);
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to != StageStates.BattleClear) return;
            BlockDragStartHandler(null);
        }

        private async void BlockDragEndHandlerAsync(BlockDragEndEvent e)
        {
            await UniTask.Delay(500);
            _rigidBody.constraints = rigidbodyConstraints;
        }

        private void BlockDragStartHandler(BlockDragStartEvent e)
        {
            rigidbodyConstraints = _rigidBody.constraints;
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void EnterDeadZoneHandler(EnterDeadZoneEvent e)
        {
            if (e.rootGoInsId != gameObject.GetInstanceID()) return;
            Dead();
        }

        //debug
        //private void Update()
        //{
        //    if(gameObject == null)
        //    {

        //    }
        //}
    }
}
