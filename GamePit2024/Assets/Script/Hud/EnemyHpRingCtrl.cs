﻿using Cysharp.Threading.Tasks;
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
                if (view == null)
                {
                    view = GetComponent<EnemyHpRingView>();
                }
                return view;
            }
        }
        public HudConfig Mode => GameData.Instance.HudConfig;

        private int rootObjId = -1;
        private int RootObjId
        {
            get
            {
                if (rootObjId == -1)
                {
                    var up = transform.GetRootParent();
                    var objId = up.gameObject.GetInstanceID();
                    rootObjId = objId;
                    //detach the hpui with enemy
                    Detach();
                }
                return rootObjId;
            }
        }

        private void Awake()
        {
            EventQueueSystem.AddListener<InitEnemyHpEvent>(InitEnemyHpHandler);
        }

        private void InitEnemyHpHandler(InitEnemyHpEvent e)
        {
            var targetId = GameManager.stageManager.GetEnemy(e.enemyId).gameObject.GetInstanceID();
            if (RootObjId != targetId) return;
            View.InitHpView(e.hp);
        }

        private void OnEnable()
        {
            EventQueueSystem.AddListener<EnemyHpChangeEvent>(EnemyHpChangeHandler);
        }

        private void EnemyHpChangeHandler(EnemyHpChangeEvent e)
        {
            //var up = transform.GetRootParent();
            //var objId = up.gameObject.GetInstanceID();
            if (RootObjId != GameManager.stageManager.GetEnemy(e.enemyId).gameObject.GetInstanceID()) return;
            View.UpdateHpView(e.lastHp, e.nowHp);
        }

        private void OnDisable()
        {
            EventQueueSystem.RemoveListener<EnemyHpChangeEvent>(EnemyHpChangeHandler);
            ResetData();
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<InitEnemyHpEvent>(InitEnemyHpHandler);
            ResetData();
        }

        private void ResetData()
        {
            view = null;
            rootObjId = -1;
        }

        private void Detach()
        {
            var par = transform.parent;
            transform.SetParent(null);
            UniTask.Void(async () =>
            {
                while (par != null && this && isActiveAndEnabled)
                {
                    transform.position = par.position + new Vector3(0.5f, 1f, -1f);
                    await UniTask.DelayFrame(1, PlayerLoopTiming.PreLateUpdate);
                }
            });
        }
    }
}