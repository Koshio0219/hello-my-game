using Deform;
using DG.Tweening;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Events;

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

        public Transform target;
        public float movingTime = 1f;
        public float closedX = -15f;
        public float openedX = 2f;
        [Range(-2f, 2f)] public float offseX = 1f;

        public Dictionary<CurtainState, UnityAction> MapStateToAction =
            new Dictionary<CurtainState, UnityAction>(3);

        void Start()
        {
            InitState();
            InitData();
        }

        private void Update()
        {
            //test
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MapStateToAction[CurtainState].Invoke();
            }
        }

        void InitState()
        {
            CurtainState = CurtainState.Closed;
            target.transform.SetLocalPositionX(closedX);
        }

        void InitData()
        {
            if (MapStateToAction.Count > 0) return;
            MapStateToAction.Add(CurtainState.Opened, CloseAction);
            MapStateToAction.Add(CurtainState.Closed, OpenAction);
            MapStateToAction.Add(CurtainState.Moving, MovingAction);
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
               CurtainState= CurtainState.Opened;
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

