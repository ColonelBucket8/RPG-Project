using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Mover mover;
        Fighter fighter;
        Health health;


        /// <summary>
        /// Handle combat and movement.
        /// Move the player within the weapon range when clicking on enemy.
        /// Do nothing when player clicks on unwalkable terrain
        /// </summary>
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            // Action Priority
            if (InteractWithCombat())
            {
                return;
            };

            if (InteractWithMovement())
            {
                return;
            }
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;


                if (!fighter.CanAttack(target.gameObject))
                {
                    continue;
                }


                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(target.gameObject);
                }

                // Put return outside to get the value when hovering on the enemy
                // To get mouse cursor interaction
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
