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
            EventQueueSystem.QueueEvent(new SceneLoadStartEvent());

            var progress = 0f;
            await SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Single).ToUniTask(Progress.Create<float>(p =>
            {
                if (progress == p)
                    return;

                progress = p;
                EventQueueSystem.QueueEvent(new SceneLoadProgressChangeEvent(progress));

                Debug.Log($"current scene loding progress is {progress * 100:F2}%");
            }));

            System.GC.Collect();

            EventQueueSystem.QueueEvent(new SceneLoadFinishedEvent());
        }
    }
}

