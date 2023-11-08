using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using UnityEngine.InputSystem;

namespace Game.Test
{
    public class Log : MonoBehaviour
    {
        public static void Info(Type instance, string text)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(GetHeader(instance) + text);
#endif
        }

        public static void Info(Type instance, UnityEngine.Object unityObject, string text)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(GetHeader(instance) + text, unityObject);
#endif
        }

        public static void Info(Type instance, string text, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogFormat(GetHeader(instance) + text, args);
#endif
        }

        public static void Info(Type instance, string text, UnityEngine.Object unityObject, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogFormat(GetHeader(instance) + text, unityObject, args);
#endif
        }

        public static void Warning(Type instance, string text, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarningFormat(GetHeader(instance) + text, args);
#endif
        }

        public static void Warning(Type instance, string text, UnityEngine.Object unityObject, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarningFormat(GetHeader(instance) + text, unityObject, args);
#endif
        }

        public static void Assertion(Type instance, string text, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogAssertionFormat(GetHeader(instance) + text, args);
#endif
        }

        public static void Assertion(Type instance, string text, UnityEngine.Object unityObject, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogAssertionFormat(GetHeader(instance) + text, unityObject, args);
#endif
        }

        public static void Error(Type instance, string text, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogErrorFormat(GetHeader(instance) + text, args);
#endif
        }

        public static void Error(Type instance, string text, UnityEngine.Object unityObject, params object[] args)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogErrorFormat(GetHeader(instance) + text, unityObject, args);
#endif
        }

        private static string GetHeader(Type type)
        {
            if (type == null)
            {
                return string.Format("[{0}]", Time.frameCount, type);
            }

            return string.Format("[{0}][{1}]", Time.frameCount, type);
        }
    }
}

