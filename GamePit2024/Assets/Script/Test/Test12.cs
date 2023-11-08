using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{

    public class Test12 : MonoBehaviour
    {

        public AsyncReactiveProperty<float> tranX;

        private void Start()
        {
            tranX.ForEachAsync(x =>
            {
                Debug.Log($"current tranX is changed ,value is :{x}");

            }, this.GetCancellationTokenOnDestroy()).Forget();

            tranX.Value = 1f;
            tranX.Value = 2f;
            tranX.Value = 2f;

            UniTask.Post(() =>
            {
                Debug.Log("UniTask.Post");
            });
        }

        //private void OnDestroy()
        //{
        //    tranX.Dispose();
        //}
    }
}


