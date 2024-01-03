using Game.Base;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Framework;
using System;
using Game.Data;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Game.Hud
{
    public class StageShowCtrl : HudCtrl<StageShowView>
    {
        private int curTime;
        private CancellationTokenSource tokenSource;

        private void Awake()
        {
            EventQueueSystem.AddListener<PlayerHpChangeEvent>(PlayerHpChangeHandler);
            EventQueueSystem.AddListener<PointChangeEvent>(PointChangeHandler);
            EventQueueSystem.AddListener<PlayerDeadEvent>(PlayerDeadHnadler);
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<PlayerHpChangeEvent>(PlayerHpChangeHandler);
            EventQueueSystem.RemoveListener<PointChangeEvent>(PointChangeHandler);
            EventQueueSystem.RemoveListener<PlayerDeadEvent>(PlayerDeadHnadler);
        }

        private void PlayerDeadHnadler(PlayerDeadEvent e)
        {
            View.FadeOut();
        }

        private void PointChangeHandler(PointChangeEvent e)
        {
            View.UpdatePointBar(e.lastP, e.nowP, e.reachGoal);
        }

        private void PlayerHpChangeHandler(PlayerHpChangeEvent e)
        {
            switch (e.playerType)
            {
                case PlayerType.Swordsman:
                    View.UpdateSwordHpbar(e.lastHp, e.nowHp);
                    break;
                case PlayerType.Witch:
                    View.UpdateWitchHpbar(e.lastHp, e.nowHp);
                    break;
            }
        }

        private void Start()
        {
            tokenSource = new CancellationTokenSource();
            InitPlayersHpbar();
            View.InitPointBar(GameManager.pointManager.GoalPoint);
            InitTimerBar();
        }

        private void InitPlayersHpbar()
        {
            foreach (var one in GameManager.stageManager.GetAllPlayer())
            {
                if (!one.TryGetComponent<Player>(out var player)) continue;
                switch (player.PlayerType)
                {
                    case PlayerType.Swordsman:
                        View.InitSwordHpbar(GameData.Instance.PlayerParameter.hp_M);
                        break;
                    case PlayerType.Witch:
                        View.InitWitchHpbar(GameData.Instance.PlayerParameter.hp_L);
                        break;
                }
            }
        }

        private void InitTimerBar()
        {
            curTime = GameData.Instance.LevelConfig.levelDatas[GameManager.Instance.LevelIdx].showTime;
            View.InitTimerBar(curTime);

            UniTask.Void(async (_) =>
            {
                while (this && isActiveAndEnabled && !_.IsCancellationRequested)
                {
                    await UniTask.Delay(1000, cancellationToken: tokenSource.Token);
                    curTime--;
                    View.UpdateTimerBar(curTime);

                    if(curTime <= 0)
                    {
                        View.FadeOut();
                        EventQueueSystem.QueueEvent(new StageTimeUpEvent());
                        break;
                    }
                }
            }, tokenSource.Token);
        }

        private void OnDisable()
        {
            tokenSource.Cancel();
        }
    }
}