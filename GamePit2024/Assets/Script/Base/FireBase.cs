using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Base
{
    public abstract class FireBase : MonoBehaviour, IFireOption
    {
        public abstract void FireBegin(int creatorId = 0, AttackState attackState = AttackState.Normal);
        public abstract void FireShut();

        public virtual void SetDamage(float damage, float damageUpSpeed = 0f, int maxDamage = 0, float exCritRate = 0f)
        {
        }

        public virtual void SetTarget(GameObject tar)
        {
        }

        public virtual void SetFirePos(Transform pos)
        {
        }
    }
}

