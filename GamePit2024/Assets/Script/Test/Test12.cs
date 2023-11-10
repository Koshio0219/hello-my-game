using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

namespace Game.Test
{

    public class Test12 : MonoBehaviour
    {

        public AsyncReactiveProperty<float> tranX;

        private Rigidbody _rigidbody;

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

            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            //_rigidbody.AddForce(Vector3.up*5, ForceMode.VelocityChange);
            _rigidbody.DOMoveY(transform.position.y + 2,1f).SetEase(Ease.OutQuad);
        }

        //private void OnDestroy()
        //{
        //    tranX.Dispose();
        //}
    }
}


