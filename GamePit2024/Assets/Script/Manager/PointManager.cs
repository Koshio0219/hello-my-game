using Game.Data;
using Game.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Manager
{
    public enum GetPointItem
    {
        KillSlime,
        KillGuard,
        KillGhost,
        GetGem,
        ReachGoal
    }

    public class PointManager
    {
        public PointManager() 
        {
            currentPoint = 0;
            mapItemToPoint.Clear();
            mapItemToPoint.AddOrSet(GetPointItem.KillSlime, 0);
            mapItemToPoint.AddOrSet(GetPointItem.KillGuard, 0);
            mapItemToPoint.AddOrSet(GetPointItem.KillGhost, 0);
            mapItemToPoint.AddOrSet(GetPointItem.GetGem, 0);
            mapItemToPoint.AddOrSet(GetPointItem.ReachGoal, 0);
        }

        private int currentPoint;
        public int CurrentPoint
        {
            get 
            {
                return currentPoint;            
            }
            set
            {
                if (value == currentPoint) return;
                var last = currentPoint;
                currentPoint = value;

                var reach = ReachGoal;
                EventQueueSystem.QueueEvent(new PointChangeEvent(last, currentPoint, reach));

                if (!reach) return;
                EventQueueSystem.QueueEvent(new ReachPointEvent());
            }
        }

        private int? goalPoint = null;
        public int GoalPoint
        {
            get
            {
                goalPoint ??= GameData.Instance.LevelConfig.levelDatas[GameManager.Instance.LevelIdx].goalPoint;
                return goalPoint.Value;
            }
        }

        public bool ReachGoal => CurrentPoint >= GoalPoint;

        public Dictionary<GetPointItem, int> mapItemToPoint = new(5);

        public int AddPoint(GetPointItem item,int point)
        {
            mapItemToPoint[item] += point;
            CurrentPoint += point;
            return CurrentPoint;
        } 
    }
}
