//Game中の全てのEventはこのScriptに書く、そして確認できる
//EventはGameEventというClassを継承する必要がある
using Game.Manager;
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