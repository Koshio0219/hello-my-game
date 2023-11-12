using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {

        protected override void Awake()
        {
            base.Awake();
            SetFrameRate(60);
            SetFixedDeltaTime(0.02f);

            EventQueueSystem.AddListener<SceneLoadStartEvent>(SceneLoadStartHandler);
        }

        private void SceneLoadStartHandler(SceneLoadStartEvent e)
        {
            Debug.Log("change scene started!");
            DOTween.KillAll();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventQueueSystem.RemoveListener<SceneLoadStartEvent>(SceneLoadStartHandler);
        }

        internal void SetFrameRate(int frameRate)
        {
            if (QualitySettings.vSyncCount != 0) QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        internal void SetFixedDeltaTime(float deltaTime)
        {
            Time.fixedDeltaTime = deltaTime;
        }
    }
}

