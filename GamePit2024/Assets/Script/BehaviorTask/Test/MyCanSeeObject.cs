using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Game.BehaviorTask
{
    public class MyCanSeeObject : Conditional
    {
        public SharedTransformList targets;

        public SharedFloat radius = 5f;
        public SharedFloat angel = 120f;

        public SharedTransformList results;

        private float sqrRadius;

        public override TaskStatus OnUpdate()
        {
            results.Value.Clear();
            foreach (var target in targets.Value)
            {
                if (target == null)
                    continue;

                Vector3 offse = target.position - transform.position;
                if (offse.sqrMagnitude > sqrRadius)
                    continue;

                float angel = Vector3.Angle(transform.forward, offse.normalized);
                if (angel > this.angel.Value * .5f)
                    continue;

                Ray ray = new Ray(transform.position, offse);
                RaycastHit info;
                if (Physics.Raycast(ray, out info, radius.Value))
                {
                    if (info.collider.gameObject == target.gameObject)
                    {
                        results.Value.Add(target);
                    }
                }
            }

            if (results.Value.Count > 0)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }

        public override void OnStart()
        {
            results.Value = new List<Transform>();
            sqrRadius = radius.Value * radius.Value;
        }
    }
}
