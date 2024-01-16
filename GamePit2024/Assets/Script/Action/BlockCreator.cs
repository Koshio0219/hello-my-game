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
using Game.Unit;
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
            EventQueueSystem.AddListener<ReachPointEvent>(ReachPointHandler);
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
            EventQueueSystem.RemoveListener<ReachPointEvent>(ReachPointHandler);
        }

        private void ReachPointHandler(ReachPointEvent e)
        {
            //List<NormalBlock> temp =new();
            //insBlocks.ForEach(one1 =>
            //{
            //    if (one1 is not NormalBlock) return;                
            //    var _temp = one1 as NormalBlock;
            //    temp.Add(_temp);                
            //});

            var prefab = GameData.Instance.LevelConfig.goalPrefab;
            var ins = Instantiate(prefab);
            ins.transform.position = GameManager.stageManager.SelecteOneBlockPoint().position;
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
                var blockUnits= blockCreateData.blockUnitDatas[i].blockUnits;
                for (int j = 0; j < blockUnits.Count; j++)
                {
                    var one = blockUnits[j];
                    if (one.baseType == BlockBaseType.Null) continue;

                    var prefab = GameData.Instance.BlockTypeConfig.GetBlockPrefab(one.useType);
                    var ins = GameObjectPool.Instance.GetObj(prefab, transform, false);
                    ins.transform.SetLocalPositionX(i * 2);
                    ins.transform.SetLocalPositionY(ins.transform.localPosition.y + one.offseY);

                    var block = ins.GetComponent<BlockBase>();
                    block.OnInstance(one);
                    insBlocks.Add(block);

                    //add block point list
                    if(block.BlockUnitData.useType == BlockUseType.Normal)
                    {
                        var temp = block as NormalBlock;
                        GameManager.stageManager.AddBlockPoint(temp.createPoint);
                    }
                    else if(block.BlockUnitData.useType == BlockUseType.GemCreate)
                    {
                        var temp = block as NormalBlock;
                        var gem = GameData.Instance.LevelConfig.gemPrefab;
                        var gem_ins = Instantiate(gem);
                        gem_ins.transform.SetParent(temp.createPoint);
                        gem_ins.transform.position = temp.createPoint.position + Vector3.up * .5f;
                    }

                    if (i == 0 && j == 0)
                    {
                        currentSelected = block;
                        block.OnSelected();
                    }
                    await UniTask.DelayFrame(1);
                }
            }
            Debug.Log("create end!");
            EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.MapBlockCreateEnd));
            //EventQueueSystem.QueueEvent(new UpdateNavMeshEvent());
        });
    }
}

