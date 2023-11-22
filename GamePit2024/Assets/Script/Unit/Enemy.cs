using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Unit
{
    public class Enemy : MonoBehaviour, IEnemyBaseAction
    {
        private EnemyUnitData enemyUnitData;
        public EnemyUnitData EnemyUnitData => enemyUnitData;

        private EnemyState state;
        private EnemyState State { get => state; set => state = value; }
        public EnemyState EnemyState => State;

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
                hp = value;
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
            Debug.Log($"Attacking! targetId:{targetId},damage:{damage}");
        }

        public virtual void Born(EnemyUnitData data)
        {
            data.Init();
            enemyUnitData = data;
            SetBaseProp(data.prop);
        }

        public virtual void Dead()
        {

        }

        public virtual void Hit(int sourceId, float damage)
        {

        }

        protected virtual void SetBaseProp(EnemyBaseProp baseProp)
        {
            MaxHp = baseProp.maxHp;
            Hp = baseProp.Hp;
            Atk = baseProp.attack;
        }
    }
}

