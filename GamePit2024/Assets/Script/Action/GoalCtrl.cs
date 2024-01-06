using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class GoalCtrl : MonoBehaviour
    {
        private readonly List<int> targetIds = new();

        private void Start()
        {
            targetIds.Clear();
            GameManager.stageManager.GetAllPlayer().ForEach(one =>
            {
                targetIds.Add(one.GetInstanceID());
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            var up = other.transform.GetRootParent();
            var id = up.gameObject.GetInstanceID();
            if (!targetIds.Contains(id)) return;
            targetIds.Remove(id);
            if (targetIds.Count > 0) return;
            EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.BattleClear));
        }
    }
}

