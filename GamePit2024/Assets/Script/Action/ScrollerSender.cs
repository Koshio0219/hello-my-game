using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    [RequireComponent(typeof(Renderer))]
    public class ScrollerSender : MonoBehaviour
    {
        public AsyncReactiveProperty<float> posX;

        public float speed = 1f;

        private Renderer renderer1;
        private Camera mainCamera;

        private void Start()
        {
            renderer1 = GetComponent<Renderer>();
            mainCamera = Camera.main;

            posX.Value = transform.position.x;

            posX.ForEachAsync(x =>
            {
                Debug.Log($"current posX is changed ,value is :{x}");

            }, this.GetCancellationTokenOnDestroy()).Forget();
        }


        void Update()
        {
            var dt = Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
            {
                var dis = dt * speed;
                transform.Translate(new Vector3(dis, 0, 0));
                //if (renderer1.IsVisableInCamera())
                if (mainCamera.IsVisableInCamera(transform.position, 0.1f))
                {
                    //posX.Value = transform.position.x;
                    EventQueueSystem.QueueEvent(new BackgroundScrollerEvent(-dis));
                }
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {
                var dis = dt * speed;
                transform.Translate(new Vector3(-dt * speed, 0, 0));
                //if (renderer1.IsVisableInCamera())
                if (mainCamera.IsVisableInCamera(transform.position, 0.1f))
                {
                    //posX.Value = transform.position.x;
                    EventQueueSystem.QueueEvent(new BackgroundScrollerEvent(dis));
                }
            }
        }
    }
}
