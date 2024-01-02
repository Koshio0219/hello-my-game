using Game.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hud
{
    public class StageShowView : HudView
    {
        public CustomBarView maleHpbar;
        public CustomBarView majoHpbar;
        public CustomBarView timerBar;
        public CustomBarView pointBar;

        private float swordMaxHp;
        private float witchMaxHp;
        private int maxPoint;
        private int maxTime;

        public void InitSwordHpbar(float hp)
        {
            maleHpbar.InitValueView($"{hp}/{hp}", hp, "Sword:");
            swordMaxHp = hp;
        }

        public void InitWitchHpbar(float hp)
        {
            majoHpbar.InitValueView($"{hp}/{hp}", hp, "Witch:");
            witchMaxHp = hp;
        }

        public void InitPointBar(int point)
        {
            pointBar.InitValueView($"{0}/{point}", point);
            maxPoint = point;
        }

        public void UpdateSwordHpbar(float lastHp,float nowHp)
        {
            maleHpbar.UpdateBarView($"{nowHp}/{swordMaxHp}", lastHp, nowHp);
        }

        public void UpdateWitchHpbar(float lastHp, float nowHp)
        {
            maleHpbar.UpdateBarView($"{nowHp}/{witchMaxHp}", lastHp, nowHp);
        }

        public void UpdatePointBar(int last,int now,bool reachGoal)
        {
            if (reachGoal)
                pointBar.UpdateBarView("exit appeared", last, now);
            else
                pointBar.UpdateBarView($"{now}/{maxPoint}", last, now);
        }
    }
}

