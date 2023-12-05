using UnityEngine;
using System.Collections.Generic;
using Game.Base;
using Game.Framework;
using UnityEditor;
using System;
using Game.Loader;
using Cysharp.Threading.Tasks;

namespace Game.Manager
{
    public class HudManager : MonoSingleton<HudManager>
    {
        private readonly Dictionary<int, HudBase> hudDic = new();

        public List<int> openList = new();

        private Transform _tran = null;
        private Transform Tran
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

        //close all of openList
        public void CloseAll()
        {
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

        public async UniTaskVoid Show(HudType _hudType, HudState _state = HudState.Hidden)
        {
            CloseAll();
            int id =await GetPanel(_hudType);
            hudDic[id].state = _state;
            hudDic[id].gameObject.Show();
            openList.Add(id);
        }

        public async UniTaskVoid AddShow(HudType _hudType, HudState _state = HudState.Destroy)
        {
            int id =await GetPanel(_hudType);
            hudDic[id].state = _state;
            hudDic[id].gameObject.Show();
            openList.Add(id);
        }

        public async UniTask<int> GetPanel(HudType _hudType)
        {
            //find exsit
            foreach (var tempHud in hudDic)
            {
                if (tempHud.Value._type == _hudType)
                {
                    return tempHud.Key;
                }
            }

            var id = GameHelper.GetId();

            HudBase _hudbase = await CreatHud(_hudType);
            _hudbase._type = _hudType;

            if (_hudbase != null)
            {
                _hudbase.Tran.SetParent(Tran);
                _hudbase.Tran.ResetLocal();

                if (!hudDic.ContainsKey(id))
                    hudDic.Add(id, _hudbase);
                else
                    Debug.Log($"exsit instance id : {id}");

                return id;
            }

            throw new Exception($"created hud is null, hudType is {_hudType}");
        }

        private async UniTask<HudBase> CreatHud(HudType _hudType)
        {
            //do creat...
            var obj =await AssetLoader.Instance.Load<HudBase>(_hudType.ToString(), this.GetCancellationTokenOnDestroy());
            return obj;
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
