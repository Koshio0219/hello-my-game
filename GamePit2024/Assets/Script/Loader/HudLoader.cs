using Game.Framework;
using Game.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Loader
{
    public class HudLoader : MonoBehaviour
    {
        private void Awake()
        {
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            switch (e.to)
            {
                case StageStates.CurtainInputStart:
                    HudManager.Instance.Show(HudType.StagePrePanel).Forget();
                    break;
                case StageStates.BattleStarted:
                    HudManager.Instance.Show(HudType.StageShowPanel).Forget();
                    break;
                case StageStates.BattleClear:
                case StageStates.GameOver:
                    HudManager.Instance.Show(HudType.StageEndPanel).Forget();
                    break;
            }
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }
    }
}

