using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Radom = UnityEngine.Random;

namespace Game.Framework
{
    /// <summary>
    /// ゲームの中でよく利用されたExtensionMethodsのクラス
    /// </summary>
    public static class ExtensionMethods
    {
        #region Collections 関連

        /// <summary>
        /// random to select one from a list
        /// </summary>
        public static T SelectOne<T>(this List<T> list)
        {
            return list[Radom.Range(0, list.Count)];
        }

        public static List<T> CutSameItem<T>(this List<T> ts)
        {
            var temp = new List<T>();
            foreach (var t in ts)
                if (!temp.Contains(t))
                    temp.Add(t);
            return temp;
        }

        public static Tk Index<Tk, Tv>(this Dictionary<Tk, Tv> dictionary, int idx)
        {
            if (dictionary.Count < idx)
            {
                throw new Exception($"the input index:{idx} is bigger with the dictionary count: {dictionary.Count}！！");
            }

            var key = dictionary.Keys.ToList()[idx];
            return key;
        }

        public static void AddOrSet<Tk, Tv>(this Dictionary<Tk, Tv> temp, Tk key, Tv value)
        {
            if (temp.ContainsKey(key))
            {
                temp[key] = value;
            }
            else
            {
                temp.Add(key, value);
            }
        }

        public static void AddOrAddValue<Tk, Tv>(this Dictionary<Tk, Tv> temp, Tk key, Tv value) where Tv : struct
        {
            if (temp.ContainsKey(key))
            {
                temp[key] = GameHelper.Add(value, temp[key]);
            }
            else
            {
                temp.Add(key, value);
            }
        }

        /// <summary>
        /// converse a dictionary（key と value　は一つ一つに対応が必要 ）
        /// </summary>
        public static Dictionary<Tv, Tk> Converse<Tk, Tv>(this Dictionary<Tk, Tv> dic)
        {
            var temp = new Dictionary<Tv, Tk>();
            foreach (var kv in dic)
            {
                if (temp.ContainsKey(kv.Value))
                {
                    Debug.LogError("key と value　は一つ一つに対応が必要");
                    temp.Clear();
                    break;
                }
                temp.Add(kv.Value, kv.Key);
            }
            return temp;
        }

        public static void Add<Tk, Item>(this Dictionary<Tk, Stack<Item>> temp, Tk tk, Item item)
        {
            if (temp.ContainsKey(tk))
            {
                temp[tk].Push(item);
            }
            else
            {
                var tempStack = new Stack<Item>();
                tempStack.Push(item);
                temp.Add(tk, tempStack);
            }
        }

        public static bool ContainsValue<Tk, Tv>(this KeyValuePair<Tk, Tv>[] tks, Tv tv)
        {
            foreach (var item in tks)
            {
                if (item.Value.Equals(tv))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsKey<Tk, Tv>(this KeyValuePair<Tk, Tv>[] tks, Tk tk)
        {
            foreach (var item in tks)
            {
                if (item.Key.Equals(tk))
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

        #region Component/Transform/GameObject 関連

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component =>
            obj.GetComponent<T>() ?? obj.AddComponent<T>();

        public static Transform GetRootParent(this Transform transform)
        {
            if (transform.parent == null)
                return transform;
            else
                return transform.parent.GetRootParent();
        }

        public static void ResetLocal(this Transform transform, bool bChangeScale = true)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if (bChangeScale)
                transform.localScale = Vector3.one;
        }

        public static int GetParentIdx<T>(this T t) where T : Component
        {
            for (int i = 0; i < t.transform.parent.childCount; i++)
            {
                var temp = t.transform.parent.GetChild(i).GetComponent<T>();
                if (temp && temp == t)
                {
                    return i;
                }
            }

            return -1;
        }

        public static void Show(this GameObject obj) 
        {
            if (obj.activeSelf) return;
            obj.SetActive(true);
        }

        public static void Hide(this GameObject obj)
        {
            if (obj.activeSelf)
                obj.SetActive(false);
        }

        public static void ShowAllChildren(this GameObject target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                target.transform.GetChild(i).gameObject.Show();
            }
        }

        public static void HideAllChildren(this GameObject target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                target.transform.GetChild(i).gameObject.Hide();
            }
        }

        public static List<GameObject> ShowChildrenCount(this GameObject target, int count)
        {
            var temp = new List<GameObject>();
            if (count > target.transform.childCount)
                return temp;

            target.HideAllChildren();
            for (int i = 0; i < count; i++)
            {
                target.transform.GetChild(i).gameObject.Show();
                temp.Add(target.transform.GetChild(i).gameObject);
            }

            return temp;
        }

        public static List<GameObject> GetAllChildren(this GameObject obj, bool includeSelf = false, bool includeHide = false)
        {
            var temp = new List<GameObject>();
            foreach (var child in obj.GetComponentsInChildren<Transform>(includeHide))
            {
                if (!includeSelf && obj.transform == child)
                {
                    continue;
                }

                temp.Add(child.gameObject);
            }

            return temp;
        }

        public static List<GameObject> GetAllParents(this GameObject obj, bool includeSelf = false)
        {
            var temp = new List<GameObject>();
            foreach (var par in obj.transform.GetComponentsInParent<Transform>())
            {
                if (!includeSelf && obj.transform == par)
                {
                    continue;
                }

                temp.Add(par.gameObject);
            }

            return temp;
        }

        public static List<GameObject> GetAllParentsAndChildren(this GameObject obj, bool includeSelf = false)
        {
            var temp = new List<GameObject>();
            var temp1 = obj.GetAllParents();
            var temp2 = obj.GetAllChildren(includeSelf);
            temp.AddRange(temp1);
            temp.AddRange(temp2);
            return temp;
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static void SetLocalPositionX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        public static void SetEulerAnglesX(this Transform transform, float x)
        {
            transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        public static void SetEulerAnglesY(this Transform transform, float y)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
        }

        public static void SetEulerAnglesZ(this Transform transform, float z)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
        }

        public static void SetLocalScaleX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        public static void SetLocalScaleY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }

        public static void SetLocalScaleZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        }

        public static Vector3 FixHeight(this Vector3 vector,float height = 0f)
        {
            return new Vector3(vector.x, height, vector.z);
        }

        #endregion

        #region Camera関連

        public static bool IsVisableInCamera(this Camera camera, Vector3 pos, float offseX = 0f, float offseY = 0f)
        {
            Vector3 viewPos = camera.WorldToViewportPoint(pos);
            if (viewPos.z < camera.nearClipPlane || viewPos.z > camera.farClipPlane) return false;

            if (offseX < 0f) offseX = 0f;
            if (offseX > 0.5f) offseX = 0.5f;
            if (offseY < 0f) offseY = 0f;
            if (offseY > 0.5f) offseY = 0.5f;

            if (viewPos.x < offseX || viewPos.y < offseY || viewPos.x > (1-offseX) || viewPos.y > (1-offseY)) return false;
            return true;
        }

        public static bool IsVisableInCamera(this Renderer renderer)
        {
            return renderer.isVisible;
        }

        #endregion

        #region UniTask関連

        public static CancellationToken GetCancellationTokenOnDisable(this Component component)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            WaitDisableTriggerAsync(component, tokenSource).Forget();
            return token;
        }

        private static async UniTask WaitDisableTriggerAsync(Component component, CancellationTokenSource tokenSource)
        {
            var disTrigger = component.GetAsyncDisableTrigger();
            await disTrigger.OnDisableAsync();
            tokenSource.Cancel();
        }

        #endregion

        #region BehaviorTree関連

        public static void SetProp(this BehaviorTree tree, string propName, object v)
        {
            if (tree == null)          
                return;
            if (tree.GetVariable(propName) == null) 
                return;
            tree.SetVariableValue(propName, v);
        }

        public static object GetProp(this BehaviorTree tree, string propName)
        {
            if (tree == null)           
                return null;
            var temp = tree.GetVariable(propName);
            if (temp == null)
                return null;

            return temp.GetValue();
        }

        #endregion
    }

}


