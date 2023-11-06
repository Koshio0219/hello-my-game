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
        private void Awake()
        {
            EventQueueSystem.AddListener<SceneLoadStartEvent>(SceneLoadStartHandler);
        }

        private void SceneLoadStartHandler(SceneLoadStartEvent e)
        {
            Debug.Log("change scene started!");
            DOTween.KillAll();
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<SceneLoadStartEvent>(SceneLoadStartHandler);
        }
    }
}

