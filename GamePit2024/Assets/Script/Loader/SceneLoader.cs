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
        private float progress = 0f;

        public async UniTaskVoid OnClickLoadScene(string toScene)
        {
            EventQueueSystem.QueueEvent(new SceneLoadStartEvent());

            await SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Single).ToUniTask(Progress.CreateOnlyValueChanged<float>(p =>
            {
                progress = p;
                EventQueueSystem.QueueEvent(new SceneLoadProgressChangeEvent(progress));

                Debug.Log($"current scene loding progress is {progress * 100:F2}%");
            }));

            System.GC.Collect();

            EventQueueSystem.QueueEvent(new SceneLoadFinishedEvent());

            progress = 0f;
        }
    }
}

