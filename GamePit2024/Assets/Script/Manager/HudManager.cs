using UnityEngine;
using System.Collections.Generic;
using Game.Base;
using Game.Framework;
using UnityEditor;
using System;

namespace Game.Manager
{
    public class HudManager : MonoSingleton<HudManager>
    {
        private Dictionary<int, HudBase> hudDic = new Dictionary<int, HudBase>();

        public List<int> openList = new List<int>();

        private Transform _tran = null;
        private Transform tran
        {
            get
            {
                if (_tran == null)
                {
                    _tran = transform;
                }
                return _tran;
            }
        }

        public void CloseAll()
        {
            //close all
            int _tempid;
            for (int i = 0; i < openList.Count; i++)
            {
                _tempid = openList[i];
                if (!hudDic.ContainsKey(_tempid))
                    continue;

                hudDic[_tempid].gameObject.Hide();
                if (hudDic[_tempid].state == HudState.Destroy)
                {
                    hudDic.Remove(_tempid);
                }
            }
        }

        public void Show(HudType _hudType, HudState _state = HudState.Hidden)
        {
            CloseAll();
            int id = GetPanel(_hudType);
            hudDic[id].state = _state;
            hudDic[id].gameObject.Show();
            openList.Add(id);
        }

        public void AddShow(HudType _hudType, HudState _state = HudState.Destroy)
        {
            int id = GetPanel(_hudType);
            hudDic[id].state = _state;
            hudDic[id].gameObject.Show();
            openList.Add(id);
        }

        public int GetPanel(HudType _hudType)
        {
            //find exsit.
            foreach (var tempHud in hudDic)
            {
                if (tempHud.Value._type == _hudType)
                {
                    return tempHud.Key;
                }
            }

            var id = GameHelper.GetId();

            HudBase _hudbase = CreatHud(_hudType);
            _hudbase._type = _hudType;

            if (_hudbase != null)
            {
                _hudbase.tran.SetParent(tran);
                _hudbase.tran.ResetLocal();

                if (!hudDic.ContainsKey(id))
                {
                    hudDic.Add(id, _hudbase);
                }
                else
                {
                    Debug.Log("exsit instance id : " + id);
                }
            }
            else
            {
                id = -1;
            }
            return id;
        }

        private HudBase CreatHud(HudType _hudType)
        {
            //do creat...
            return null;
        }
    }

    public enum HudType
    {
        None,
        Menu_Panel,
        Setting_Panel,
        Loading_Panel,
        Stage_Panel
    }
}
