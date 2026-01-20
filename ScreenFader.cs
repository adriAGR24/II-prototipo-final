using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance; 
    public CanvasGroup faderCanvasGroup; 
    public float fadeSpeed = 2.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator FadeOut()
    {
        while (faderCanvasGroup.alpha < 1)
        {
            faderCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        faderCanvasGroup.alpha = 1;
    }

    public IEnumerator FadeIn()
    {
        while (faderCanvasGroup.alpha > 0)
        {
            faderCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        faderCanvasGroup.alpha = 0;
    }
}