using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;

namespace Game.Test
{
    public class Test1 : MonoBehaviour
    {
        public Button loadScene;

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
                SceneLoader.Instance.OnClickLoadScene("Stage");
                loadScene.onClick.RemoveAllListeners();
            });
        }
    }
}