﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Framework
{
    public static class GameHelper
    {
        #region 数学関連
        public static T Add<T>(T a, T b) where T : struct
        {
            //l2cppならばこの書き方を変える必要
            dynamic x1 = a;
            dynamic x2 = b;
            return (T)(x1 + x2);
        }

        private static int temp_id;
        public static int GetId()
        {
            return temp_id++;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 euler)
        {
            Vector3 direction = point - pivot;
            Vector3 rotatedDirection = Quaternion.Euler(euler) * direction;
            Vector3 rotatedPoint = rotatedDirection + pivot;
            return rotatedPoint;
        }

        public static Vector3 RotateDirectionByPivot(Vector3 direction, Vector3 pivot, Vector3 euler)
        {
            Vector3 rotatedDirection = Quaternion.Euler(euler) * direction;
            Vector3 rotatedPoint = rotatedDirection + pivot;
            return rotatedPoint;
        }

        #endregion

        #region Enum 関連

        public static T GetEnumIdx<T>(int idx) where T : Enum
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(idx);
        }

        public static int EnumLength<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public static T GetEnumPos<T>(int pos) where T : Enum
        {
            var array = Enum.GetValues(typeof(T));
            var length = array.Length;

            for (int i = 0; i < length; i++)
            {
                var temp = (T)array.GetValue(i);
                if (pos == Convert.ToInt32(temp))
                {
                    return temp;
                }
            }

            throw new Exception("can not get the enum pos：" + pos);
        }

        #endregion

        #region Ray
        public static bool ShootRay(Vector3 orgin, Vector3 dir, float dis, string tag, Action<RaycastHit> callback)
        {
            RaycastHit info;
            if (Physics.Raycast(orgin, dir, out info, dis))
            {
                if (info.collider.tag == tag)
                {
                    if (callback != null)
                        callback(info);
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}

