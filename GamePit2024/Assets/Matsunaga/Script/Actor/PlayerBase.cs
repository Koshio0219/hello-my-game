using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Base
{
    interface PlayerBase : IDamageable
    {
        void Dead();
        void Attack(int targetID, float damage);
        void Move();
        void Hit(int sourceId, float damage);
    }

}
