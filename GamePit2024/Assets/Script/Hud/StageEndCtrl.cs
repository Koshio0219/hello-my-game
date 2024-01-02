using Game.Base;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hud
{
    public class StageEndCtrl : HudCtrl<StageEndView>
    {
        private void Start()
        {
            switch (GameManager.stageManager.StageState)
            {
                case StageStates.BattleClear:
                    View.Win();
                    break;
                case StageStates.GameOver:
                    View.Lose();
                    break;
            }
        }
    }
}

