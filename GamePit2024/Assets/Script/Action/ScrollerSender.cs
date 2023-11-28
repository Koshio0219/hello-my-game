using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Data;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Action
{
    public class ScrollerSender : MonoBehaviour
    {
        public AsyncReactiveProperty<float> posX;
        public float speed = 1f;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;

            posX.Value = transform.position.x;

            posX.ForEachAsync(x =>
            {
                Debug.Log($"current posX is changed ,value is :{x}");

            }, this.GetCancellationTokenOnDestroy()).Forget();


            var temp = UniTask.Action(async (_) =>
            {
                while (true)
                {
                    //...//
                    UpdateAction();
                    await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
                }

            }, this.GetCancellationTokenOnDestroy());
            temp.Invoke();

            
        }

        private void Awake()
        {
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to != StageStates.BattleStarted) return;

            //var temp = UniTask.UnityAction(async (_) =>
            //{
            //    while (true)
            //    {
            //        //...//
            //       await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
            //    }

            //},this.GetCancellationTokenOnDestroy());
        }

        void UpdateAction()
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
