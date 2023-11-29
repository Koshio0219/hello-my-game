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
            SceneLoader.Instance.BackToMenu();
        }

        private void BattleClearEndHandler()
        {
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
        }

        private bool IsLastStage()
        {

            return true;
        }

        private void NextStage()
        {

        }

        private void Win()
        {
            //...something else...
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
    }
}

