using Game.Data;
using Game.Framework;
using Game.Loader;
using Game.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        private int levelIdx;
        public int LevelIdx 
        {
            get
            {
                return levelIdx;
            }
            set
            {
                levelIdx = value;
            }
        }

        private void Awake()
        {
            GameManager.stageManager = this;
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
            InitMap();
        }

        private void OnDestroy()
        {
            GameManager.stageManager = null;
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
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

        private void GameOverHandler()
        {
            //lose
            //...something else...
            Debug.Log($"Game Over!");
            SceneLoader.Instance.BackToMenu();
        }

        private void BattleClearEndHandler()
        {
            Debug.Log($"battle clear !");
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

        private bool IsLastStage()
        {
            return LevelIdx >= GameData.Instance.EnemyCreateConfig.levelEnemyData.Count - 1;
        }

        private void NextStage()
        {
            LevelIdx++;
            Debug.Log($"next stage! current level idx is {LevelIdx}");
            //...something else...
        }

        private void Win()
        {
            //...something else...
            Debug.Log($"game win !");
            SceneLoader.Instance.BackToMenu();
        }

        public void AddOnePlayer(int playerId,GameObject playerIns)
        {
            if (MapPlayerIdToInstance.ContainsKey(playerId)) return;
            MapPlayerIdToInstance.Add(playerId, playerIns);
        }

        public void RemoveOnePlayer(int playerId,bool bDestroy = true) 
        {
            if (!MapPlayerIdToInstance.ContainsKey(playerId)) return;
            if (bDestroy) Destroy(MapEnemyIdToInstance[playerId]);
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

        public void Timer(StageTimerEvent e)
        {

        }
    }
}

