using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Data;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Unit
{
    public class NormalBlock : BlockBase
    {
        public Transform createPoint;
        public MeshRenderer meshRenderer;
        public Image mark;

        private readonly List<Transform> curTriggerEnter = new();
        private static bool isDraging = false;
        private bool isCheckingStay = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isDraging) return;
            if (other.transform.parent == createPoint || 
                other.transform.parent == transform ||
                other.transform.parent == transform.parent) 
                return;

            var up = other.transform.GetRootParent();
            if (curTriggerEnter.Contains(up)) return;
            curTriggerEnter.Add(up);
        }

        private void OnTriggerExit(Collider other)
        {
            if (isDraging) return;

            if (curTriggerEnter.Contains(other.transform))
            {
                curTriggerEnter.Remove(other.transform);
                return;
            }

            var up = other.transform.GetRootParent();
            if (curTriggerEnter.Contains(up))
            {
                curTriggerEnter.Remove(up);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isCheckingStay) return;
            OnTriggerEnter(other);
            isCheckingStay = false;
        }

        public override async void OnMovingStart()
        {
            base.OnMovingStart();
            isCheckingStay = true;
            await UniTask.DelayFrame(2);
            isDraging = true;
            if (curTriggerEnter.Count == 0) return;
            curTriggerEnter.ForEach(one => 
            {
                var colliders = one.GetComponentsInChildren<Collider>().ToList();
                colliders.ForEach(c => c.enabled = false);
                if (one.TryGetComponent<NavMeshAgent>(out var agent)) 
                {
                    agent.destination = agent.transform.position;
                    agent.autoTraverseOffMeshLink = false;
                    agent.autoRepath = false;
                    agent.isStopped = true;
                    agent.enabled = false;
                }
                //if (one.TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = false;
                one.SetParent(createPoint);
            });
        }

        public override async void OnMovingEnd()
        {
            base.OnMovingEnd();
            await UniTask.Delay(500);
            curTriggerEnter.ForEach(one => 
            {
                var lastPos = one.transform.position;
                one.SetParent(null);
                one.transform.position = lastPos;
                var colliders = one.GetComponentsInChildren<Collider>().ToList();
                colliders.ForEach(c => c.enabled = true);
                if (one.TryGetComponent<NavMeshAgent>(out var agent)) 
                {
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.autoRepath = true;
                    agent.autoTraverseOffMeshLink = true;
                }
                //if (one.TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = true;
            });
            await UniTask.Delay(100);
            isDraging = false;
            curTriggerEnter.Clear();
        }

        public override void OnInstance(BlockUnitData blockUnitData)
        {
            base.OnInstance(blockUnitData);
            SetMark();
            SetColor();
        }

        private void SetMark()
        {
            if (mark == null) return;
            mark.gameObject.Show();
            switch (BlockUnitData.baseType)
            {
                case BlockBaseType.Static:
                    mark.gameObject.Hide();
                    break;
                case BlockBaseType.AutoMoving:
                    mark.sprite = GameData.Instance.HudConfig.allMoveMark;
                    break;
                case BlockBaseType.UpDownAble:
                    mark.sprite = GameData.Instance.HudConfig.udMark;
                    break;
                case BlockBaseType.LeftRightAble:
                    mark.sprite = GameData.Instance.HudConfig.lrMark;
                    break;
            }
        }

        private void SetColor()
        {
            if (meshRenderer == null) return;
            if(BlockUnitData.baseType == BlockBaseType.Static)
            {
                meshRenderer.material.SetColor("_BaseColor", GameData.Instance.HudConfig.blockStatic);
            }
            else
            {
                meshRenderer.material.SetColor("_BaseColor", GameData.Instance.HudConfig.blockMoveable);
            }
        }
    }
}

