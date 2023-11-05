using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Framework;

namespace Game.Loader
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public async void OnClickLoadScene(string toScene)
        {
            await SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Single).ToUniTask(Progress.Create<float>(p =>
            {
                //something else ...
                Debug.Log($"current scene loding progress is {p * 100:F2}%");
            }));

            System.GC.Collect();
        }
    }
}

