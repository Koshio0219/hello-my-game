using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Framework;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Game.Unit
{
    [System.Serializable]
    public class BulletProp
    {
        public float speed;

        public float lifeTime = 3f;

        public float angleSpeed = 120f;

        public float acceleration = 30f;

        public float maxSpeed = 600f;
    }

    public class Bullet : MonoBehaviour
    {
        public BulletProp prop;
        public GameObject Target { get;private set; }

        private CancellationToken token = CancellationToken.None;

        private System.Action recycleAction = null;
        private System.Action moveAction = null;

        public void Init(GameObject target)
        {
            Target = target;
            token = this.GetCancellationTokenOnDestroy();
            InitAction();
            recycleAction.Invoke();
            moveAction.Invoke();
        }

        private void InitAction()
        {
             recycleAction = UniTask.Action(async (_) =>
             {
                 await UniTask.Delay((int)(prop.lifeTime * 1000));
                 GameObjectPool.Instance.RecycleObj(gameObject);
             }, token);
            moveAction = UniTask.Action(async (_) =>
            {
                while (isActiveAndEnabled)
                {
                    Move();
                    await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
                }
            }, token);
        }

        private void Move()
        {
            float deltaTime = Time.fixedDeltaTime;
            if (Target != null && prop.angleSpeed > 0)
            {
                Vector3 offset = (Target.transform.position - transform.position).normalized;

                float angle = Vector3.Angle(transform.forward, offset);

                float needTime = angle * 1.0f / prop.angleSpeed;

                transform.forward = Vector3.Lerp(transform.forward, offset, deltaTime / needTime).normalized;
            }
            if (prop.speed < prop.maxSpeed)
            {
                prop.speed += deltaTime * prop.acceleration;
            }
            transform.position += transform.forward * prop.speed * deltaTime;
        }

        private void OnDisable()
        {
            recycleAction = null;
            moveAction = null;
            token = CancellationToken.None;
        }
    }
}