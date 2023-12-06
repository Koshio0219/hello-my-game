using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Base;
using Game.Data;
using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;
//using UnityEngine.Pool;

namespace Game.Action
{
    public class BlockCreator : MonoBehaviour
    {
        public BlockCreateData blockCreateData;

        private List<BlockBase> insBlocks = new();

        private IBlockBaseAction currentSelected;

#if DEBUG_MODE
        private void Start()
        {
            StartCoroutine(Creat(blockCreateData));
        }
#endif

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
            if (e.to != StageStates.MapBlockCreateStart) return;
            if (blockCreateData == null)
            {
                Debug.LogError("blockCreateData is null,please check");
                EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.MapBlockCreateEnd));
                return;
            }

            StartCoroutine(Creat(blockCreateData));
        }

        public IEnumerator Creat(BlockCreateData blockCreateData) => UniTask.ToCoroutine(async () =>
        {
            Debug.Log("create start!");
            if (blockCreateData == null) return;
            for (int i = 0; i < blockCreateData.blockUnitDatas.Count; i++)
            {
                var one = blockCreateData.blockUnitDatas[i];
                if (one.baseType == BlockBaseType.Null) continue;

                var prefab = GameData.Instance.BlockTypeConfig.GetBlockPrefab(one.useType);
                var ins = GameObjectPool.Instance.GetObj(prefab, transform, false);
                ins.transform.SetLocalPositionX(i * 2);

                var block = ins.GetComponent<BlockBase>(); 
                block.OnInstance(one);
                insBlocks.Add(block);

                if (i == 0)
                {
                    currentSelected = block;
                    block.OnSelected();
                }
                await UniTask.DelayFrame(1);
            }
            Debug.Log("create end!");
            EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.MapBlockCreateEnd));
            EventQueueSystem.QueueEvent(new UpdateNavMeshEvent());
        });
    }
}

