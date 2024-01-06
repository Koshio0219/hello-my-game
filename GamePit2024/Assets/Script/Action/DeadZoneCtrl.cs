using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class DeadZoneCtrl : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var up = other.transform.GetRootParent();
            EventQueueSystem.QueueEvent(new EnterDeadZoneEvent(up.gameObject.GetInstanceID()));
        }
    }
}

