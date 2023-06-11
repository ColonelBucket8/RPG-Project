using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        TextMeshProUGUI textLabel;

        // Start is called before the first frame update
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            textLabel = GetComponent<TextMeshProUGUI>();

        }

        // Update is called once per frame
        private void Update()
        {
            textLabel.text = String.Format("{0:0}", experience.GetPoints());
        }
    }

}
