using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {

        CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

        }


        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                // moving alpha toward 1
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }

        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                // moving alpha toward 1
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }

        }
    }
}