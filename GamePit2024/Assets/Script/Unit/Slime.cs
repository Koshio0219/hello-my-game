﻿using Game.Data;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Unit
{
    public class Slime : Enemy
    {
        public Face faces;
        public GameObject smileBody;
        public int damType;

        private Material faceMaterial;

        public override void Born(EnemyUnitData data)
        {
            base.Born(data);
            faceMaterial = smileBody.GetComponent<Renderer>().materials[1];
        }

        //protected override void InitBehaviorTree()
        //{
        //    base.InitBehaviorTree();
        //}

        void SetFace(Texture tex)
        {
            faceMaterial.SetTexture("_MainTex", tex);
        }

        void SetSpeed(float speed = 0f)
        {
            animator.SetFloat("Speed", speed);
        }

        protected override void OnChangeIdle()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
            SetFace(faces.Idleface);
            SetSpeed();
        }

        protected override void OnChangeMove()
        {
            SetFace(faces.WalkFace);
            SetSpeed(1.2f);
        }

        protected override void OnChangeAttack()
        {
            switch (EnemyAttackState)
            {
                case AttackState.Normal:
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) break;
                    SetFace(faces.attackFace);
                    animator.SetTrigger("Attack");
                    SetSpeed();
                    break;
                case AttackState.Skill:
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
                    SetFace(faces.jumpFace);
                    animator.SetTrigger("Jump");
                    SetSpeed();
                    break;
            }
        }

        protected override void OnChangeHit()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;

            animator.SetTrigger("Damage");
            animator.SetInteger("DamageType", damType);
            SetFace(faces.damageFace);
            SetSpeed();
        }
    }
}