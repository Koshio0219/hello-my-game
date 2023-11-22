using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAttackEffect : StateMachineBehaviour
{
    /// <summary>
    ///    攻撃待機エフェクト
    /// </summary>
    [SerializeField] private GameObject _AttackReady;
    [SerializeField] private GameObject _AttackReadyEnd;
    private GameObject _AttackReadyEffect;
    private GameObject _AttackReadyEndEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _AttackReadyEffect = GameObject.Instantiate(_AttackReady, animator.gameObject.transform.position, Quaternion.identity) as GameObject;
        _AttackReadyEffect.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(_AttackReadyEffect);
        _AttackReadyEndEffect = GameObject.Instantiate(_AttackReadyEnd, animator.gameObject.transform.position, Quaternion.identity) as GameObject;
        _AttackReadyEndEffect.SetActive(true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //    // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

