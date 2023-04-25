using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;

        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < weaponRange)
            {
                mover.Stop();
            }
            else
            {
                mover.MoveTo(target.position);
            }

        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        /// <summary>
        /// Cancel and stop attacking
        /// </summary>
        public void Cancel()
        {
            target = null;
        }
    }
}