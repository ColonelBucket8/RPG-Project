using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{

    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI damageText = null;

        public void DestroyText() { Destroy(gameObject); }

        public void SetValue(float damageAmount)
        {
            damageText.text = damageAmount.ToString();
        }
    }
}
