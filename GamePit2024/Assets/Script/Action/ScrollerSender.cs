using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Base;
using Game.Data;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Action
{
    public class ScrollerSender : MonoBehaviour,IInit
    {
        [SerializeField] private Transform target;
        private AsyncReactiveProperty<float> targetPosx;
        private float lastPosx;

        public float speed = 1f;
        private Camera mainCamera;
        private System.Action updateAction = null;
        private System.Action initAction = null;

        private void Start()
        {
            mainCamera = Camera.main;
            if (target == null) target = transform;
            initAction = Init;

            updateAction = UniTask.Action(async (_) =>
            {
                while (this && isActiveAndEnabled)
                {
                    UpdateAction();
                    await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
                }
            }, this.GetCancellationTokenOnDestroy());       
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
            initAction?.Invoke();
            updateAction?.Invoke();
        }

        private void UpdateAction()
        {
            targetPosx.Value = target.position.x;
        }

        private void PosxChangeHandler(float posx)
        {
            if (!mainCamera.IsVisableInCamera(target.transform.position, 0.1f))
                return;

            var dis = posx - lastPosx;
            EventQueueSystem.QueueEvent(new BackgroundScrollerEvent(-dis * speed));     
        }

        public void Init()
        {
            lastPosx = transform.position.x;
            targetPosx = new AsyncReactiveProperty<float>(lastPosx);
            targetPosx.ForEachAsync(x =>
            {
                //Debug.Log($"target:{gameObject.name} current posx is changed ,value is :{x}");
                if (lastPosx == x) return;
                PosxChangeHandler(x);
                lastPosx = x;

            }, this.GetCancellationTokenOnDestroy()).Forget();
            initAction = null;
        }

        #region test
        //void UpdateAction()
        //{
        //    var dt = Time.deltaTime;
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        var dis = dt * speed;
        //        transform.Translate(new Vector3(dis, 0, 0));
        //        //if (renderer1.IsVisableInCamera())
        //        if (mainCamera.IsVisableInCamera(transform.position, 0.1f))
        //        {
        //            //posX.Value = transform.position.x;
        //            EventQueueSystem.QueueEvent(new BackgroundScrollerEvent(-dis));
        //        }
        //        return;
        //    }

        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        var dis = dt * speed;
        //        transform.Translate(new Vector3(-dt * speed, 0, 0));
        //        //if (renderer1.IsVisableInCamera())
        //        if (mainCamera.IsVisableInCamera(transform.position, 0.1f))
        //        {
        //            //posX.Value = transform.position.x;
        //            EventQueueSystem.QueueEvent(new BackgroundScrollerEvent(dis));
        //        }
        //    }
        //}
        #endregion
    }
}
