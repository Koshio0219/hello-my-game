using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class GameObjectPool : Singleton<GameObjectPool>
    {
        private const int maxCount = 128;
        private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

        private GameObject _pool = null;

        public GameObject GetObj(GameObject perfab)
        {
            var _name = perfab.name;
            var result = CheckPool(_name);

            //Poolの中にある場合
            if (result != null)
            {
                return result;
            }
 
            //Poolの中にない場合
            result = Object.Instantiate(perfab);
            result.name = _name;
            return result;
        }

        public GameObject GetObj(string name)
        {
            GameObject result = CheckPool(name);
            return result == null ? new GameObject(name) : result;
        }

        public GameObject GetObj(GameObject perfab, Transform parent, bool worldPositionStays = true)
        {
            var result = GetObj(perfab);
            result.transform.SetParent(parent, worldPositionStays);
            return result;
        }

        private GameObject CheckPool(string name)
        {
            GameObject result = null;
            if (pool.ContainsKey(name))
            {
                if (pool[name].Count > 0)
                {
                    result = pool[name][0];
                    if (result != null)
                    {
                        result.Show();
                        pool[name].Remove(result);
                    }
                    else
                    {
                        pool.Remove(name);
                    }
                }
            }
            return result;
        }

        public void RecycleObj(GameObject obj, bool worldPositionStays = true)
        {
            if (obj != null && (!obj.activeSelf)) return;
            if (_pool == null)
            {
                _pool = new GameObject("_objectPool_");
            }
            obj.transform.SetParent(_pool.transform, worldPositionStays);
            obj.Hide();

            if (pool.ContainsKey(obj.name))
            {
                if (pool[obj.name].Count < maxCount)
                {
                    pool[obj.name].Add(obj);
                }
            }
            else
            {
                pool.Add(obj.name, new List<GameObject>() { obj });
            }
        }

        public void RecycleAllChildren(GameObject parent)
        {
            for (; parent.transform.childCount > 0;)
            {
                var tar = parent.transform.GetChild(0).gameObject;
                RecycleObj(tar);
            }
        }

        public void Clear()
        {
            pool.Clear();
            Object.Destroy(_pool);
            _pool = null;
        }
    }
}
