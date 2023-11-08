using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using Game.Framework;
using System;

namespace Game.Test
{
    public class Test1 : MonoBehaviour,IProgress<float>
    {
        public Button loadScene;

        private void Awake()
        {
            EventQueueSystem.AddListener<SceneLoadProgressChangeEvent>(SceneLoadProgressChangeHandler);
        }

        private void SceneLoadProgressChangeHandler(SceneLoadProgressChangeEvent e)
        {
            //something else ...
            //Debug.Log($"current scene loding progress is {e.progress * 100:F2}%");
        }

        void Start()
        {
            gameObject.transform.DOMoveX(transform.position.x + 5, 1.0f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                Debug.Log($"the game object {gameObject.name} has move to its x+5 position.");
            });

            CheckLoad();
        }

        private void CheckLoad()
        {
            if (loadScene == null) return;

            loadScene.onClick.AddListener(() =>
            {
                SceneLoader.Instance.OnClickLoadScene("Stage").Forget();
                loadScene.onClick.RemoveAllListeners();
            });
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<SceneLoadProgressChangeEvent>(SceneLoadProgressChangeHandler);
        }

        public void Report(float value)
        {
            //throw new NotImplementedException();
        }
    }
}