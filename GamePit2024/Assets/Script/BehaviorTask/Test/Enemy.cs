using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviorTask
{
    public enum EnemyType
    {
        Close,
        Remote,
        Boss
    }

    public class Enemy : MonoBehaviour
    {
        public EnemyType enemyType;
        public float hp = 100f;

        public void Hit(float damage)
        {
            hp -= damage;
        }

        public void Dead()
        {
            Destroy(gameObject);
        }

    }
}

