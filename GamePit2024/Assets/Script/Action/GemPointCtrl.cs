using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class GemPointCtrl : MonoBehaviour
    {
        public List<int> point = new(3);
        private void OnTriggerEnter(Collider other)
        {
            var up = other.transform.GetRootParent();
            if (!GameManager.stageManager.GetAllPlayer().Contains(up.gameObject)) return;
            GameManager.pointManager.AddPoint(GetPointItem.GetGem, point.SelectOne());
            Destroy(gameObject);
        }
    }
}

