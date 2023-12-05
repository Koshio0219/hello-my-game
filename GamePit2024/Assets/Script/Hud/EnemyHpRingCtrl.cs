using Game.Base;
using Game.Data;
using Game.Framework;
using Game.Hud;
using Game.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Hud
{
    [RequireComponent(typeof(EnemyHpRingView))]
    public class EnemyHpRingCtrl : MonoBehaviour
    {
        private EnemyHpRingView view = null;
        public EnemyHpRingView View 
        { 
            get
            {
                if(view == null)
                {
                    view = GetComponent<EnemyHpRingView>();
                }
                return view;
            }
        }
        public HudConfig Mode => GameData.Instance.HudConfig;

        private void Start()
        {
            view.InitHpView();
        }

        private void OnEnable()
        {
            EventQueueSystem.AddListener<EnemyHpChangeEvent>(EnemyHpChangeHandler);
        }

        private void EnemyHpChangeHandler(EnemyHpChangeEvent e)
        {
            var up = transform.GetRootParent();
            var objId = up.gameObject.GetInstanceID();
            if (objId != GameManager.stageManager.GetEnemy(e.enemyId).GetInstanceID()) return;
            view.UpdateHpView(e.lastHp, e.nowHp);
        }

        private void OnDisable()
        {
            EventQueueSystem.RemoveListener<EnemyHpChangeEvent>(EnemyHpChangeHandler);
        }
    }
}