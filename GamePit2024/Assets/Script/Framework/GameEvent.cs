//Game中の全てのEventはこのScriptに書く、そして確認できる
//EventはGameEventというClassを継承する必要がある
using Game.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class TestGameEvent : GameEvent
    {
        public string test;

        public TestGameEvent(string test)
        {
            this.test = test;
        }
    }

    public class SceneLoadStartEvent : GameEvent { }

    public class SceneLoadProgressChangeEvent : GameEvent
    {
        public float progress;

        public SceneLoadProgressChangeEvent(float progress)
        {
            this.progress = progress;
        }
    }

    public class SceneLoadFinishedEvent : GameEvent { }

    public class BackgroundScrollerEvent : GameEvent 
    {
        public float dis;

        public BackgroundScrollerEvent(float dis)
        {
            this.dis = dis;
        }
    }

    public class StageStatesEvent : GameEvent
    {
        public StageStates to;
        public StageStatesEvent(StageStates stageStates)
        {
            to = stageStates;
        }
    }

    public enum DamageActonType
    {
        Trigger,
        PointTo,
        Range
    }

    public class SendDamageEvent : GameEvent
    {
        public DamageActonType damageActonType;
        public int sourceId;
        //type PointTo
        public int targetId;
        //type Trigger
        public GameObject enterObj;
        //type Range
        public List<GameObject> rangeObjs; 
        public float damage;

        //type trigger
        public SendDamageEvent(int sourceId, GameObject enterObj, float damage)
        {
            this.sourceId = sourceId;
            this.enterObj = enterObj;
            this.damage = damage;
            damageActonType = DamageActonType.Trigger;
        }

        //type pointto
        public SendDamageEvent(int sourceId, int targetId, float damage)
        {
            this.sourceId = sourceId;
            this.targetId = targetId;
            this.damage = damage;
            damageActonType = DamageActonType.PointTo;
        }

        //type range
        public SendDamageEvent(int sourceId, List<GameObject> rangeObjs, float damage)
        {
            this.sourceId = sourceId;
            this.rangeObjs = rangeObjs;
            this.damage = damage;
            damageActonType = DamageActonType.Range;
        }
    }

    //public class OnGameStartEvent:GameEvent 
    //{
    //    public int playerId;
    //    public string nick;
    //    public OnGameStartEvent(int playerId, string nick)
    //    {
    //        this.playerId = playerId;
    //        this.nick = nick;
    //    }
    //}
}