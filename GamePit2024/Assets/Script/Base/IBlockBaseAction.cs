using System.Collections;
using UnityEngine;

namespace Game.Base
{
    interface IBlockBaseAction
    {
        public BlockBaseType BaseType { get; }
        public BlockState State { get; }

        void OnInstance();
        void OnSelected();
        void OnMovingStart();
        //void OnMoving();
        void OnMovingEnd();
        void OnDeselected();
        void OnRemove();   
    }
}