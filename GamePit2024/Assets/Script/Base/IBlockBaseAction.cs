using Game.Data;
using System.Collections;
using UnityEngine;

namespace Game.Base
{
    interface IBlockBaseAction
    {
        public BlockUnitData BlockUnitData { get; }
        public BlockState State { get; }

        void OnInstance(BlockUnitData blockUnitData);
        void OnSelected();
        void OnMovingStart();
        //void OnMoving();
        void OnMovingEnd();
        void OnDeselected();
        void OnRemove();   
    }
}