using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 20f;

        bool isDead = false;
        CapsuleCollider capsuleCollider;
        NavMeshAgent navMeshAgent;

        private void Start()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            // Avoid reducing health below than zero
            // If health minus damage is less than zero
            // take zero and assign it to health
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
            }

        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
            capsuleCollider.enabled = false;
            navMeshAgent.enabled = false;
        }
    }
}