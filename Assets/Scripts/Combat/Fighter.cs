using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = 0f;

        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.IsDead()) return;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < weaponRange)
            {
                mover.Cancel();
                AttackBehaviour();
            }
            else
            {
                mover.MoveTo(target.transform.position);
            }

        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                // This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        /// <summary>
        /// Cancel and stop attacking
        /// </summary>
        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        // Animation Event 
        void Hit()
        {
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
    }
}