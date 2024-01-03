using Cysharp.Threading.Tasks;
using Deform;
using DG.Tweening;
using Game.Data;
using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Action
{
    public enum CurtainState
    {
        Opened,
        Closed,
        Moving
    }

    [RequireComponent(typeof(Deformable))]
    public class CurtainCtrl : MonoBehaviour
    {
        private CurtainState curtainState;
        public CurtainState CurtainState { get => curtainState; set => curtainState = value; }

        public float selfX = 8f;
        public Transform target;
        public float movingTime = 1f;
        public float closedX = -15f;
        public float openedX = 2f;
        [Range(-2f, 2f)] public float offseX = 1f;

        public readonly Dictionary<CurtainState, UnityAction> MapStateToAction = new(3);

        private bool inputable = false;
        private int inputIdx = -1;

        private void Awake()
        {
            InitState();
            InitData();

            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
            EventQueueSystem.AddListener<StageTimeUpEvent>(StageTimeUpHandler);
            EventQueueSystem.AddListener<PlayerDeadEvent>(PlayerDeadHandler);
        }

        private void PlayerDeadHandler(PlayerDeadEvent e)
        {
            StageTimeUpHandler(null);
        }

        private async void StageTimeUpHandler(StageTimeUpEvent e)
        {
            MapStateToAction[CurtainState].Invoke();
            if (selfX < 0) return;
            await UniTask.Delay((int)(movingTime * 1000));
            EventQueueSystem.QueueEvent(new StageStatesEvent(Manager.StageStates.GameOver));
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to != Manager.StageStates.CurtainInputStart) return;
            inputable = true;
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
            EventQueueSystem.RemoveListener<StageTimeUpEvent>(StageTimeUpHandler);
            EventQueueSystem.RemoveListener<PlayerDeadEvent>(PlayerDeadHandler);
        }

        private void Update()
        {
#if DEBUG_MODE
            //test
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MapStateToAction[CurtainState].Invoke();
            }
#endif
            if (!inputable) return;
            if (Gamepad.all.Count > 0 && inputIdx >= 0 && Gamepad.all[inputIdx] != null)
            {
                var input = Gamepad.all[inputIdx].startButton.wasPressedThisFrame;
                //var input = Gamepad.all[inputIdx].startButton.isPressed;
                if (!input) return;
                MapStateToAction[CurtainState].Invoke();
                inputable = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MapStateToAction[CurtainState].Invoke();
                    inputable = false;
                }
            }
        }

        void InitState()
        {
            inputIdx = -1;
            inputable = false;
            CurtainState = CurtainState.Closed;
            transform.SetLocalPositionX(selfX);
            target.transform.SetLocalPositionX(closedX);
        }

         void InitData()
        {
            if (MapStateToAction.Count > 0) return;
            MapStateToAction.Add(CurtainState.Opened, CloseAction);
            MapStateToAction.Add(CurtainState.Closed, OpenAction);
            MapStateToAction.Add(CurtainState.Moving, MovingAction);

            var data =  GameData.Instance.PlayerParameter;
            inputIdx = data.GamepadNumber_D;
        }

        void OpenAction()
        {
            if (CurtainState != CurtainState.Closed) return;

            CurtainState = CurtainState.Moving;

            if (offseX != 0f)
            {
                transform.DOLocalMoveX(transform.localPosition.x + offseX, movingTime);
            }

            target.DOLocalMoveX(openedX, movingTime).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                CurtainState = CurtainState.Opened;

                if (selfX > 0) EventQueueSystem.QueueEvent(new StageStatesEvent(Manager.StageStates.CurtainInputEnd));
            });
        }

        void CloseAction()
        {
            if (CurtainState != CurtainState.Opened) return;
            CurtainState = CurtainState.Moving;

            if (offseX != 0f)
            {
                transform.DOLocalMoveX(transform.localPosition.x - offseX, movingTime);
            }

            target.DOLocalMoveX(closedX, movingTime).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                CurtainState = CurtainState.Closed;
            });
        }

        void MovingAction()
        {

        }
    }
}

