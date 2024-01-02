using Game.Base;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Framework;
using System;

namespace Game.Hud
{
    public class StageShowCtrl : HudCtrl<StageShowView>
    {
        private void Awake()
        {
            EventQueueSystem.AddListener<PlayerHpChangeEvent>(PlayerHpChangeHandler);
            EventQueueSystem.AddListener<PointChangeEvent>(PointChangeHandler);
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<PlayerHpChangeEvent>(PlayerHpChangeHandler);
            EventQueueSystem.RemoveListener<PointChangeEvent>(PointChangeHandler);
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
            InitPlayersHpbar();
            View.InitPointBar(GameManager.pointManager.GoalPoint);
        }

        private void InitPlayersHpbar()
        {
            foreach (var one in GameManager.stageManager.GetAllPlayer())
            {
                if (!one.TryGetComponent<Player>(out var player)) continue;
                switch (player.PlayerType)
                {
                    case PlayerType.Swordsman:
                        View.InitSwordHpbar(player.Hp);
                        break;
                    case PlayerType.Witch:
                        View.InitWitchHpbar(player.Hp);
                        break;
                }
            }
        }
    }
}

