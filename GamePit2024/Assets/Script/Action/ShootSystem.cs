using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Game.Base;
using Game.Data;
using Game.Loader;
using Game.Unit;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Action
{
    public class ShootSystem : FireBase
    {
        public AssetReference bulletPrefab;
        public Transform firePos;
        public GameObject target;

        public float damage;
        //public int sourceId;

        public override async void FireBegin(int creatorId = 0, AttackState attackState = AttackState.Normal)
        {
            //test
            //move
            var bullet = await AssetLoader.Instance.Load<Bullet>(bulletPrefab, this.GetCancellationTokenOnDestroy());
            bullet.transform.SetParent(null);
            bullet.transform.position = firePos.position;
            bullet.transform.forward = firePos.forward;
            bullet.Init(target);

            //hit
            var hit = bullet.GetComponent<BulletHit>();
            var enterTrigger = hit.GetAsyncTriggerEnterTrigger();
            var enter = await enterTrigger.OnTriggerEnterAsync(hit.GetCancellationTokenOnDestroy());
            hit.OnEnterHit(enter, creatorId, damage);

            //recycle
            bullet.Recycle();

            //hit.sourceId = creatorId;
            //hit.damage = damage;
        }

        public override void FireShut()
        {

        }

        public override FireBase SetTarget(GameObject tar)
        {
            target = tar;
            return base.SetTarget(tar);
        }

        public override FireBase SetFirePos(Transform pos)
        {
            firePos = pos;
            return base.SetFirePos(pos);
        }

        public override FireBase SetDamage(float damage, float damageUpSpeed = 0, int maxDamage = 0, float exCritRate = 0)
        {
            this.damage = damage;
            return base.SetDamage(damage, damageUpSpeed, maxDamage, exCritRate);
        }
    }
}