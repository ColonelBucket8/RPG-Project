using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI textLabel;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            textLabel = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textLabel.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(),
                                           health.GetMaxHealthPoints());
        }
    }

}
