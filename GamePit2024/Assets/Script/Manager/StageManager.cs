using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Framework;
using Game.Loader;
using Game.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics.Platform;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Manager
{
    public enum StageStates
    {
        GetReady,
        MapBlockCreateStart,
        MapBlockCreateEnd,
        NavMeshBuildStart,
        NavMeshBuildEnd,
        CurtainInputStart,
        CurtainInputEnd,
        EnemyBuildStart,
        EnemyBuildEnd,
        BattleStarted,
        BattleClear,//win
        GameOver//lose
    }

    public class StageManager : MonoBehaviour
    {
        private StageStates stageState = StageStates.GetReady;
        public StageStates StageState { get => stageState; private set => stageState = value; }

        private readonly Dictionary<StageStates, UnityAction> mapStateToEvent = new();

        private Dictionary<int, GameObject> MapPlayerIdToInstance { get; set; } = new();
        private Dictionary<int, Enemy> MapEnemyIdToInstance { get; set; } = new();

        private List<Transform> blockPoints = new();

        private void Awake()
        {
            GameManager.stageManager = this;
            GameManager.pointManager = new PointManager();
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
            InitMap();
        }

        private void OnDestroy()
        {
            GameManager.stageManager = null;
            GameManager.pointManager = null;
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
            blockPoints.Clear();
        }

        private void InitMap()
        {
            if (mapStateToEvent.Count > 0) return;
            mapStateToEvent.Add(StageStates.MapBlockCreateEnd, MapBlockCreateEndHandler);
            mapStateToEvent.Add(StageStates.NavMeshBuildEnd, NavMeshBuildEndHandler);
            mapStateToEvent.Add(StageStates.CurtainInputEnd, CurtainInputEndHandler);
            mapStateToEvent.Add(StageStates.EnemyBuildEnd, EnemyBuildEndHandler);
            mapStateToEvent.Add(StageStates.BattleClear, BattleClearEndHandler);
            mapStateToEvent.Add(StageStates.GameOver, GameOverHandler);
        }

        private async void GameOverHandler()
        {
            //lose
            //...something else...
            Debug.Log($"Game Over!");
            ClearAllEnemies();
            ClearAllPlayers();

            await UniTask.Delay(1000);
            GameManager.Instance.LevelIdx = 0;
            this.WaitInput(Gamepad.current.circleButton, () => SceneLoader.Instance.BackToMenu());
        }

        private void BattleClearEndHandler()
        {
            Debug.Log($"battle clear !");
            ClearAllEnemies();
            GameManager.pointManager.AddPoint(GetPointItem.ReachGoal, GameManager.Instance.CurLevelTime);

            if (IsLastStage())
            {
                Win();
            }
            else
            {
                NextStage();
            }
        }

        private void EnemyBuildEndHandler()
        {
            SendStateEvent(StageStates.BattleStarted);
        }

        private void CurtainInputEndHandler()
        {
            SendStateEvent(StageStates.EnemyBuildStart);
        }

        private void NavMeshBuildEndHandler()
        {
            SendStateEvent(StageStates.CurtainInputStart);
        }

        private void MapBlockCreateEndHandler()
        {
            SendStateEvent(StageStates.NavMeshBuildStart);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (mapStateToEvent.Count == 0) return;
            if (!mapStateToEvent.ContainsKey(e.to)) return;
            mapStateToEvent[e.to].Invoke();
            StageState = e.to;
        }

        private void Start()
        {
            StageState = StageStates.GetReady;
            SendStateEvent(StageStates.MapBlockCreateStart);
        }

        private void SendStateEvent(StageStates state)
        {
            EventQueueSystem.QueueEvent(new StageStatesEvent(state));
            StageState = state;
            Debug.Log($"current stage state is :{state}");
        }

        public bool IsLastStage()
        {
            return GameManager.Instance.LevelIdx >= GameData.Instance.LevelConfig.levelDatas.Count - 1;
        }

        private async void NextStage()
        {
            //...wait ui show...
            await UniTask.Delay(1000);
            GameManager.Instance.LevelIdx++;
            Debug.Log($"next stage! current level idx is {GameManager.Instance.LevelIdx}");
            this.WaitInput(Gamepad.current.circleButton, () => SceneLoader.Instance.GoToStage());
        }

        private async void Win()
        {
            Debug.Log($"game win !");
            // wait ui show
            await UniTask.Delay(1000);
            GameManager.Instance.LevelIdx = 0;
            this.WaitInput(Gamepad.current.circleButton, () => SceneLoader.Instance.BackToMenu());
        }

        public void AddOnePlayer(int playerId,GameObject playerIns)
        {
            if (MapPlayerIdToInstance.ContainsKey(playerId)) return;
            MapPlayerIdToInstance.Add(playerId, playerIns);
        }

        public void RemoveOnePlayer(int playerId,bool bDestroy = true) 
        {
            if (!MapPlayerIdToInstance.ContainsKey(playerId)) return;
            if (bDestroy) Destroy(MapPlayerIdToInstance[playerId]);
            MapPlayerIdToInstance.Remove(playerId);
        }

        public void ClearAllPlayers()
        {
            foreach (var item in MapPlayerIdToInstance)
            {
                Destroy(item.Value);
            }
            MapPlayerIdToInstance.Clear();
        }

        public void AddOneEnemy(int enemyId,Enemy enemy)
        {
            if (MapEnemyIdToInstance.ContainsKey(enemyId)) return;
            MapEnemyIdToInstance.Add(enemyId, enemy);
        }

        public void RemoveOneEnemy(int enemyId,bool bDestroy = true)
        {
            if (!MapEnemyIdToInstance.ContainsKey(enemyId)) return;
            if (bDestroy) Destroy(MapEnemyIdToInstance[enemyId].gameObject);
            MapEnemyIdToInstance.Remove(enemyId);
        }

        public void ClearAllEnemies()
        {
            foreach (var item in MapEnemyIdToInstance)
            {
                Destroy(item.Value.gameObject);
            }
            MapEnemyIdToInstance.Clear();
        }

        public GameObject GetPlayer(int playerId)
        {
            if (!MapPlayerIdToInstance.ContainsKey(playerId)) return null;
            return MapPlayerIdToInstance[playerId];
        }

        public Enemy GetEnemy(int enemyId)
        {
            if (!MapEnemyIdToInstance.ContainsKey(enemyId)) return null;
            return MapEnemyIdToInstance[enemyId];
        }

        public Enemy FindCloseEnemy(Vector3 pos)
        {
            if (MapEnemyIdToInstance.Count == 0) return null;
            var result = MapEnemyIdToInstance[MapEnemyIdToInstance.Index(0)];
            var offse = pos - result.transform.position;
            var dis = Vector3.SqrMagnitude(offse);
            for(int i = 1; i < MapEnemyIdToInstance.Count; i++)
            {
                var one = MapEnemyIdToInstance[MapEnemyIdToInstance.Index(i)];
                var temOffse = pos - one.transform.position;
                var temDis = Vector3.SqrMagnitude(temOffse);
                if (temDis < dis)
                {
                    dis = temDis;
                    result = one;
                }
            }
            return result;
        }

        public int MatchPlayerId(GameObject target)
        {
            foreach (var item in MapPlayerIdToInstance)
            {
                if (target.GetInstanceID() == item.Value.GetInstanceID())
                {
                    return item.Key;
                }
            }

            Debug.Log($"match player failure! target name :{target.name}");
            return -1;
        }

        public bool IsFriend(int id1,int id2)
        {
            return (MapPlayerIdToInstance.ContainsKey(id1) && MapPlayerIdToInstance.ContainsKey(id2)) || (MapEnemyIdToInstance.ContainsKey(id1) && MapEnemyIdToInstance.ContainsKey(id2));
        }

        public List<GameObject> GetAllPlayer() => MapPlayerIdToInstance.Values.ToList();
   
        public void AddBlockPoint(Transform one)
        {
            if (blockPoints.Contains(one)) return;
            blockPoints.Add(one);
        }

        public Transform SelecteOneBlockPoint() => blockPoints.SelectOne();
    }
}

