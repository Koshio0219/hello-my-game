using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class AnimationEventHandler : MonoBehaviour
    {
        // slime animation
        public void AlertObservers(string message)
        {

            if (message.Equals("AnimationDamageEnded"))
            {
                //Debug.Log("DamageAnimationEnded");
            }

            if (message.Equals("AnimationAttackEnded"))
            {

            }

            if (message.Equals("AnimationJumpEnded"))
            {

            }
        }
    }
}

