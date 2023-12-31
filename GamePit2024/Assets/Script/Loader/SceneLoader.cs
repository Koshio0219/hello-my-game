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
        private bool isLoading = false;

        private async UniTaskVoid OnClickLoadScene(string toScene)
        {
            EventQueueSystem.QueueEvent(new SceneLoadStartEvent());
            isLoading = true;

            await SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Single).ToUniTask(Progress.CreateOnlyValueChanged<float>(p =>
            {
                progress = p;
                EventQueueSystem.QueueEvent(new SceneLoadProgressChangeEvent(progress));

                Debug.Log($"current scene loding progress is {progress * 100:F2}%");
            }));

            System.GC.Collect();

            EventQueueSystem.QueueEvent(new SceneLoadFinishedEvent());
            isLoading = false;

            progress = 0f;
        }

        public void BackToMenu()
        {
            if (isLoading) return;
            OnClickLoadScene("Start").Forget();
        }

        public void GoToStage()
        {
            if (isLoading) return;
            OnClickLoadScene("Stage").Forget();
        }
    }
}

