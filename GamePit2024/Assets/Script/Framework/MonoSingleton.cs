using Game.Base;
using UnityEngine;

namespace Game.Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour,IInit where T : MonoBehaviour
    {
        private static T instance = null;

        private static readonly object locker = new object();

        private static bool bAppQuitting;

        public static T Instance
        {
            get
            {
                if (bAppQuitting)
                {
                    instance = null;
                    return instance;
                }

                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError($"singleton ¤ÏÒ»¤Ä¤·¤«¤Ê¤¤¤Ï¤º¤À£¡" +
                                $"{instance.gameObject.name}¤È¤¤¤¦gameObject¤ò—Ê–Ë¤¹¤Ù¤­¤À£¡");
                            return instance;
                        }

                        if (instance == null)
                        {
                            var singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton)" + typeof(T);
                            singleton.hideFlags = HideFlags.None;
                            DontDestroyOnLoad(singleton);
                        }
                        else
                        {
                            instance.hideFlags = HideFlags.None;
                            DontDestroyOnLoad(instance.gameObject);
                        }
                    }
                    //instance.hideFlags = HideFlags.None;
                    return instance;
                }
            }
        }

        protected virtual void Awake()
        {
            bAppQuitting = false;
        }

        protected virtual void OnDestroy()
        {
            bAppQuitting = true;
        }

        //init the Instance on start
        public virtual void Init()
        {
        }
    }
}

