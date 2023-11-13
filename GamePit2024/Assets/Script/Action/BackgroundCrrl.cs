using Game.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class BackgroundCrrl : MonoBehaviour
    {
        public float speed;
        private float x;
        public float leftLine;
        public float rightLine;

        private void Awake()
        {
            EventQueueSystem.AddListener<BackgroundScrollerEvent>(BackgroundScrollerHandler);
        }

        private void BackgroundScrollerHandler(BackgroundScrollerEvent e)
        {
            var dis = e.dis * speed;
            transform.Translate(new Vector3(dis, 0, 0));

            x = transform.position.x;
            if (x < leftLine)
            {
                x = rightLine;
                transform.SetPositionX(x);
                return;
            }

            if (x > rightLine)
            {
                x = leftLine;
                transform.SetPositionX(x);
            }
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<BackgroundScrollerEvent>(BackgroundScrollerHandler);
        }
    }
}

