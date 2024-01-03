using Game.Framework;
using Game.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Loader
{
    [RequireComponent(typeof(HudManager))]
    public class HudLoader : MonoBehaviour
    {
        private HudManager hudManager;
        private void Awake()
        {
            hudManager = GetComponent<HudManager>();
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            switch (e.to)
            {
                case StageStates.CurtainInputStart:
                    hudManager.Show(HudType.StagePrePanel).Forget();
                    break;
                case StageStates.BattleStarted:
                    hudManager.Show(HudType.StageShowPanel).Forget();
                    break;
                case StageStates.BattleClear:
                case StageStates.GameOver:
                    hudManager.Show(HudType.StageEndPanel).Forget();
                    break;
            }
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }
    }
}

