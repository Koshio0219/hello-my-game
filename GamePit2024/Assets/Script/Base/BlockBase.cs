using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Base
{
    public enum BlockBaseType
    {
        Null,
        Static,
        AutoMoving,
        UpDownAble,
        LeftRightAble
    }

    public enum BlockState
    {
        Normal,
        Selected,
        Moving
    }

    public class BlockBase : MonoBehaviour,IBlockBaseAction
    {
        private BlockUnitData blockUnitData;
        public BlockUnitData BlockUnitData => blockUnitData;

        private BlockState state;
        protected BlockState BlockState { get => state; set => state = value; }

        public BlockState State => BlockState;

        public virtual void OnDeselected()
        {
            BlockState = BlockState.Normal;
        }

        public virtual void OnInstance(BlockUnitData blockUnitData)
        {
            BlockState = BlockState.Normal;
            this.blockUnitData = blockUnitData;
        }

        public virtual void OnRemove()
        {
            BlockState = BlockState.Normal;
        }

        public virtual void OnSelected()
        {
            BlockState = BlockState.Selected;
        }

        public virtual void OnMovingStart()
        {
            BlockState = BlockState.Moving;
        }

        public virtual void OnMovingEnd()
        {
            BlockState = BlockState.Selected;
        }
    }
}

