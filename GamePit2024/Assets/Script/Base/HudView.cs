using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Base
{
    public class HudView : MonoBehaviour
    {
        public HudState state;
        public HudType _type;

        public float Z_deep = 0;

        private Transform _tran = null;
        public Transform Tran
        {
            get
            {
                if(_tran == null)
                {
                    _tran = transform;
                }
                return _tran;
            }
        }

        public void OnEnable()
        {
            OnShow();
        }

        public void OnDisable()
        {
            OnClose();
        }

        public virtual void OnShow()
        {

        }

        public virtual void OnClose()
        {
            if (state == HudState.Hidden)
                gameObject.Hide();
            else if (state == HudState.Destroy)
                GameObjectPool.Instance.RecycleObj(gameObject);           
        }

        public virtual void OnRemove() { }

    }

    public enum HudState
    {
        None,
        Hidden,
        Show,
        Destroy,
    }
}
