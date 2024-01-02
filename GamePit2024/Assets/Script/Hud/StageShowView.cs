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

        public void InitSwordHpbar(float hp)
        {
            maleHpbar.InitValueView($"{hp}/{hp}", hp, "Sword");
            swordMaxHp = hp;
        }

        public void InitWitchHpbar(float hp)
        {
            majoHpbar.InitValueView($"{hp}/{hp}", hp, "Witch");
            witchMaxHp = hp;
        }

        public void UpdateSwordHpbar(float lastHp,float nowHp)
        {
            maleHpbar.UpdateBarView($"{nowHp}/{swordMaxHp}", lastHp, nowHp);
        }

        public void UpdateWitchHpbar(float lastHp, float nowHp)
        {
            maleHpbar.UpdateBarView($"{nowHp}/{witchMaxHp}", lastHp, nowHp);
        }
    }
}

