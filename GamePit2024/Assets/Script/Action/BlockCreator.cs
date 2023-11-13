using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Base;
using Game.Data;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;

namespace Game.Action
{
    public class BlockCreator : MonoBehaviour
    {
        public BlockCreateData blockCreateData;

        private List<BlockBase> insBlocks = new List<BlockBase>();

        private IBlockBaseAction currentSelected;

        private void Start()
        {
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

                var prefab = GameData.Instance.blockTypeConfig.GetBlockPrefab(one.useType);
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

        });
    }
}

