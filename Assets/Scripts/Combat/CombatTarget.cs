using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType() { return CursorType.Combat; }

        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();

            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }

            // Put return outside to get the value when hovering on the enemy
            // To get mouse cursor interaction
            return true;
        }
    }
}
