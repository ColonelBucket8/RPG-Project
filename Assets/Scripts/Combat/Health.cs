using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public void TakeDamage(float damage)
        {
            // Avoid reducing health below than zero
            // If health minus damage is less than zero
            // take zero and assign it to health
            health = Mathf.Max(health - damage, 0);
            print(health);
        }
    }
}