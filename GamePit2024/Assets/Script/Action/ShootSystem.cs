using Cysharp.Threading.Tasks;
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

        public override async void FireBegin(int creatorId = 0, AttackState attackState = AttackState.Normal)
        {
            //test
            var bullet = await AssetLoader.Instance.Load<Bullet>(bulletPrefab, this.GetCancellationTokenOnDestroy());
            bullet.transform.SetParent(null);
            bullet.transform.position = firePos.position;
            bullet.transform.forward = firePos.forward;
            bullet.Init(target);
        }

        public override void FireShut()
        {

        }

        public override void SetFirePos(Transform pos)
        {
            firePos = pos;
        }

        public override void SetTarget(GameObject tar)
        {
            target = tar;
        }

        public override void SetDamage(float damage, float damageUpSpeed = 0, int maxDamage = 0, float exCritRate = 0)
        {
            
        }
    }
}