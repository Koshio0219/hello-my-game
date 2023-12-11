using Cysharp.Threading.Tasks;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Loader
{
    public enum AssetType
    {
        Prefab,
        Sprite,
        Texture,
        AnimationClip,
        AudioClip,
        Material,
        Config
        //others...
    }

    public class AssetLoader : Singleton<AssetLoader>
    {
        private readonly Dictionary<AssetType, string> mapTypeToPath = new Dictionary<AssetType, string>()
        {
            { AssetType.Prefab,""},
            { AssetType.Sprite,""},
            { AssetType.Texture,""},
            { AssetType.AnimationClip,""},
            { AssetType.AudioClip,""},
            { AssetType.Material,""},
            { AssetType.Config,""},
             //others...
        };

        public async UniTask<T> Load<T>(AssetType type, string name, CancellationToken token)
        {
            var path = mapTypeToPath[type];
            var wPath = Path.Combine(path, name);
            var result = await Addressables.LoadAssetAsync<T>(wPath).WithCancellation(token);

            return result;
        }

        public async UniTask<T> Load<T>(string name, CancellationToken token, bool usePool = true) where T : MonoBehaviour
        {
            //var res = Load<GameObject>(AssetType.Prefab, name, token).GetAwaiter().GetResult();
            var res = await Load<GameObject>(AssetType.Prefab, name, token);
            var obj = usePool ? GameObjectPool.Instance.GetObj(res) : Object.Instantiate(res);

            if (obj.TryGetComponent<T>(out var t))
                return t;

            throw new System.Exception($"GetComponent is error. component name:{typeof(T)}");
        }

        public async UniTask<T> Load<T>(IKeyEvaluator key, CancellationToken token)
        {
            var result = await Addressables.LoadAssetAsync<T>(key).WithCancellation(token);
            return result;
        }

        public async UniTask<T> Load<T>(AssetReference key, CancellationToken token, bool usePool = true) where T : MonoBehaviour
        {
            var res = await Load<GameObject>(key, token);
            var obj = usePool ? GameObjectPool.Instance.GetObj(res) : Object.Instantiate(res);

            if (obj.TryGetComponent<T>(out var t))
                return t;

            throw new System.Exception($"GetComponent is error. component name:{typeof(T)}");
        }

        public void Release<T>(T t)
        {
            Addressables.Release(t);
        }
    }
}