using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            damageTextPrefab.SetValue(damageAmount);
            Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }

}
