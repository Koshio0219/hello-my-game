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
        public float MaxHp
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
        public float Hp
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
        public float Atk
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

        public void Attack(int targetId, float damage)
        {

        }

        public void Born(EnemyUnitData data)
        {

        }

        public void Dead()
        {

        }

        public void Hit(int sourceId, float damage)
        {

        }
    }
}

