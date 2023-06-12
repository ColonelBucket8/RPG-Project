using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        TextMeshProUGUI textLabel;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            textLabel = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textLabel.text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }

}
