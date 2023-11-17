using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using Game.Framework;
using System;
using Game.Unit;
using Cysharp.Threading.Tasks;
using Game.Data;
using UnityEngine.AddressableAssets;

namespace Game.Test
{
    public class Test1 : MonoBehaviour,IProgress<float>
    {
        public Button loadScene;

        public AssetReference asset;

        private void Awake()
        {
            EventQueueSystem.AddListener<SceneLoadProgressChangeEvent>(SceneLoadProgressChangeHandler);
        }

        private void SceneLoadProgressChangeHandler(SceneLoadProgressChangeEvent e)
        {
            //something else ...
            //Debug.Log($"current scene loding progress is {e.progress * 100:F2}%");
        }

        async void Start()
        {
            //gameObject.transform.DOMoveX(transform.position.x + 5, 1.0f).SetEase(Ease.InCubic).OnComplete(() =>
            //{
            //Debug.Log($"the game object {gameObject.name} has move to its x+5 position.");
            //});

           // Debug.Log($"asset path:{asset.SubObjectName}");

            CheckLoad();

            var token = this.GetCancellationTokenOnDestroy();
            //var ins=await AssetLoader.Instance.Load<NormalBlock>("Assets/Prefab/NormalBlock.prefab", token);
            var ins=await AssetLoader.Instance.Load<NormalBlock>(asset, token);

            var config = await AssetLoader.Instance.Load<BlockTypeConfig>(AssetType.Config,"Assets/Config/BlockTypeConfig.asset", token);

            Debug.Log($"load mono completed! name is:{ins.name}");
            Debug.Log($"load config completed! name is:{config.GetBlockPrefab(BlockUseType.Normal).name}");
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