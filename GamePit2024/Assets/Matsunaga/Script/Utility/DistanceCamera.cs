using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using UnityEngine.InputSystem;

namespace Game.Test
{
    public class DistanceCamera : MonoBehaviour
    {
        public Transform target; //カメラが追従する対象オブジェクトのtransform
        [SerializeField] private Transform camera; //カメラのtransform
        private float horizontal2;
        private float vertical2;
        private float theta;                    //方位角(スティック左右)
        [SerializeField] public float phi = 21f;                      //仰角(スティック上下)
        [SerializeField] public float rotateSpeed = 3.0f;//カメラを回転させる速度
        private float eulerRotate;
        [SerializeField] private float distance;
        [SerializeField] private float eyeHeight;
        [SerializeField] private Vector3 offset;
        [SerializeField] private int controller;

        void Start()
        {
            //最初のカメラの位置はキャラクターの指定距離(distance)分真後ろにしたい
            MoveBehind();
        }


        void MoveBehind()
        {
            eulerRotate = target.rotation.eulerAngles.y;
            camera.position = target.position + eyeHeight * Vector3.up - target.rotation * Vector3.forward * distance;
            theta = 0f;
            phi = 12f;
            float x = distance * Mathf.Cos(phi * Mathf.Deg2Rad) * Mathf.Sin(theta * Mathf.Deg2Rad);
            float z = distance * Mathf.Cos(phi * Mathf.Deg2Rad) * Mathf.Cos(theta * Mathf.Deg2Rad);
            camera.position = new Vector3(//モデルの角度をオブジェクトに合わせる: euilerRotate
                        x * Mathf.Cos(eulerRotate * Mathf.Deg2Rad) - z * Mathf.Sin(eulerRotate * Mathf.Deg2Rad),
                        distance * Mathf.Sin(phi * Mathf.Deg2Rad),
                        -(x * Mathf.Sin(eulerRotate * Mathf.Deg2Rad) + z * Mathf.Cos(eulerRotate * Mathf.Deg2Rad))
                    ) + target.position + Vector3.up * eyeHeight;

            camera.LookAt(target.position + Vector3.up * eyeHeight);//中心点を常に見る
            offset = target.position - camera.position;
            //Gamepad.all[0].leftStick.ReadValue().x
        }

        void Update()
        {
            float horizontal = Gamepad.current.rightStick.ReadValue().x;
            float vertical = Gamepad.current.rightStick.ReadValue().y;
            theta -= horizontal * rotateSpeed;
            phi += vertical * rotateSpeed;
            theta %= 360;
            phi = Mathf.Clamp(phi % 360, -10f, 40f);
            float x = distance * Mathf.Cos(phi * Mathf.Deg2Rad) * Mathf.Sin(theta * Mathf.Deg2Rad);
            float z = distance * Mathf.Cos(phi * Mathf.Deg2Rad) * Mathf.Cos(theta * Mathf.Deg2Rad);
            camera.position = new Vector3(//モデルの角度をオブジェクトに合わせる: euilerRotate
                        x * Mathf.Cos(eulerRotate * Mathf.Deg2Rad) - z * Mathf.Sin(eulerRotate * Mathf.Deg2Rad),
                        distance * Mathf.Sin(phi * Mathf.Deg2Rad),
                        -(x * Mathf.Sin(eulerRotate * Mathf.Deg2Rad) + z * Mathf.Cos(eulerRotate * Mathf.Deg2Rad))
                    ) + target.position + Vector3.up * eyeHeight;

            camera.LookAt(target.position + Vector3.up * eyeHeight);//中心点を常に見る
            offset = target.position - camera.position;
        }
    }
}

