using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter target;
        TextMeshProUGUI textLabel;

        private void Awake()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            textLabel = GetComponent<TextMeshProUGUI>();

        }

        private void Update()
        {
            if (target.GetTarget() == null)
            {
                textLabel.text = "N/A";

            }
            else
            {
                textLabel.text = String.Format("{0:0.0}%", target.GetTarget().GetPercentage());
            }
        }
    }

}
