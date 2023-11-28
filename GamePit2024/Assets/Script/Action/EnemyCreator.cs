using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Data;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Action
{
    public class EnemyCreator : MonoBehaviour,IInit
    {
        private EnemyCreateConfig enemyCreateConfig;

        private List<IEnemyBaseAction> insEnemy = new();

        private UnityAction buildAction = null;

#if DEBUG_MODE
        private void Start()
        {
            buildAction.Invoke();
        }
#endif

        private void Awake()
        {
            Init();
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        private void OnDestroy()
        {
            buildAction = null;
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to != StageStates.EnemyBuildStart) return;
            if (enemyCreateConfig == null)
            {
                Debug.LogError("enemyCreateConfig is null,please check");
                EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.EnemyBuildEnd));
                return;
            }

            buildAction.Invoke();
        }

        public void Init()
        {
            enemyCreateConfig = GameData.Instance.EnemyCreateConfig;
            buildAction = UniTask.UnityAction(async (_) =>
            {
                Debug.Log("enemy build start!");
                if (enemyCreateConfig == null) return;
                for (int i = 0; i < enemyCreateConfig.EnemyConfig.Count; i++)
                {
                    var one = enemyCreateConfig.EnemyConfig[i];

                    //
                    //..build and init..
                    //

                    await UniTask.DelayFrame(1);
                }
                Debug.Log("enemy build end!");
                EventQueueSystem.QueueEvent(new StageStatesEvent(StageStates.EnemyBuildEnd));
            },this.GetCancellationTokenOnDestroy());
        }
    }
}

