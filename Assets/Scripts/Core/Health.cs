using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int health = 100;
        Animator animator;
        ActionScheduler actionScheduler;
        private bool isDead = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void DealDamage(int damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health <= 0 && !isDead)
            {
                Die();
            }
            Debug.Log("health: " + health);
        }

        private void Die()
        {
            isDead = true;
            animator.SetTrigger("death");
            actionScheduler.CancelCurrentAction();
        }
    }

}