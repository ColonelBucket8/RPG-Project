using UnityEngine;

namespace RPG.Attributes
{

    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        Health healthComponent = null;
        [SerializeField]
        RectTransform foreground = null;
        [SerializeField]
        Canvas rootCanvas = null;

        void Update()
        {
            showHealthBar();

            foreground.localScale =
                new Vector3(healthComponent.GetFraction(), 1, 1);
        }

        private void showHealthBar()
        {
            float healthFraction = healthComponent.GetFraction();

            if (!Mathf.Approximately(healthFraction, 1f) &&
                !Mathf.Approximately(healthFraction, 0f))
            {
                rootCanvas.gameObject.SetActive(true);
            }
            else
            {
                rootCanvas.gameObject.SetActive(false);
            }
        }
    }
}
