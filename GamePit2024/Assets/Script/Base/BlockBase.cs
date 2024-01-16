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
        public Rigidbody Rigidbody { get; private set; }

        public virtual void OnDeselected()
        {
            BlockState = BlockState.Normal;
        }

        public virtual void OnInstance(BlockUnitData blockUnitData)
        {
            BlockState = BlockState.Normal;
            this.blockUnitData = blockUnitData;
            if (!TryGetComponent<Rigidbody>(out var rigidbody)) return;
            switch (blockUnitData.baseType)
            {
                case BlockBaseType.Static:
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    break;
                case BlockBaseType.AutoMoving:
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                    rigidbody.freezeRotation = true;
                    break;
                case BlockBaseType.UpDownAble:
                    rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;
                    break;
                case BlockBaseType.LeftRightAble:
                    rigidbody.constraints = ~RigidbodyConstraints.FreezePositionX;
                    break;
            }
            Rigidbody = rigidbody;
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
            Rigidbody.isKinematic = false;
            Rigidbody.mass = 1;
        }

        public virtual void OnMovingEnd()
        {
            BlockState = BlockState.Selected;
            Rigidbody.mass = 1000;
        }

        public virtual void OnMovingReset()
        {
            Rigidbody.isKinematic = true;
        }

        protected virtual void OnDestroy()
        {
            OnRemove();
        }
    }
}

