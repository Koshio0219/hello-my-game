using Game.Base;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class RemoteEnemy : Enemy
    {
        public FireBase fire;

        public override void Attack(int targetId, float damage)
        {
            if (fire == null) return;
            var target = GameManager.stageManager.GetPlayer(targetId);
            var creatorId = EnemyUnitData.InsId;

            fire.SetTarget(target).
                SetFirePos(fire.transform).
                SetDamage(damage+Atk).
                FireBegin(creatorId);
            base.Attack(targetId, damage);
        }
    }
}

