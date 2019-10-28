using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.5f;
        [SerializeField] int damage = 10;
        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Mover mover;
        Animator animator;
        ActionScheduler actionScheduler;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!target.IsDead())
            {
                if (!GetIsInRange())
                {
                    mover.MoveTo(target.transform.position,1f);
                }
                else
                {
                    mover.Cancel();
                    if (timeSinceLastAttack >= timeBetweenAttacks)
                    {
                        timeSinceLastAttack = 0f;
                        AttackBehavior();
                    }
                    
                }
            }                  
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        //Triggered in Animation, trigger for animation set by AttackBehavior()
        private void Hit()
        {
            if (target == null) { return; }
            target.DealDamage(damage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject target)
        {
            actionScheduler.StartAction(this);
            Debug.Log("Attacking.");
            this.target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            target = null;
        }
    }
}
