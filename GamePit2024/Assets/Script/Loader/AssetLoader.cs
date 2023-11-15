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
             //others...
        };

        public async UniTask<T> Load<T>(AssetType type,string name, CancellationToken token)
        {
            var path = mapTypeToPath[type];
            var wPath= Path.Combine(path, name);
            var result = await Addressables.LoadAssetAsync<T>(wPath).WithCancellation(token);
            return result;
        }

        public T Load<T>(string name, CancellationToken token) where T:MonoBehaviour
        {
            var res = Load<T>(AssetType.Prefab, name, token).GetAwaiter().GetResult();
            var obj = GameObjectPool.Instance.GetObj(res as GameObject);
            
            if (obj.TryGetComponent<T>(out var t))                           
                return t;
            
            throw new System.Exception($"GetComponent is error. component name:{typeof(T)}");
        }
    }
}