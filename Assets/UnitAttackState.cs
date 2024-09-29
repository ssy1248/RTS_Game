using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f;

    public float attackRate = 1f;
    private float attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();

        if(attackController.muzzleEffect.gameObject != null)
        {
            attackController.muzzleEffect.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("∏”¡Ò ¿Ã∆Â∆Æ ºº∆√ æ»µ ");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false) 
        {
            LookAtTarget();

            //Keep moving towards enemy
            //agent.SetDestination(attackController.targetToAttack.position);

            if(attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            // Should unit still attack
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                animator.SetBool("isAttacking", false); // Move to Attacking state
            }
        }
        else
        {
            animator.SetBool("isAttacking", false); // Move to Attacking state
        }
    }

    private void Attack()
    {
        var damageToInflict = attackController.unitDamage;

        SoundManager.Instance.PlayInfantryAttackSound();

        // Actually Attack Unit
        attackController.targetToAttack.GetComponent<Unit>().TakeDamage(damageToInflict);
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.muzzleEffect != null && attackController.muzzleEffect.gameObject != null)
        {
            attackController.muzzleEffect.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Muzzle Effect is not assigned or is missing on exit.");
        }
    }
}
