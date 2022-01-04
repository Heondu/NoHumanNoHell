using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{
    private Image image;
    private Coroutine fadeCoroutine;
    private float fadeTime;

    private void Start()
    {
        image = GetComponent<Image>();

        FadeOut(1);
    }

    public void FadeIn(float fadeTime)
    {
        this.fadeTime = fadeTime;
        
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(0, 1));
    }

    public void FadeOut(float fadeTime)
    {
        this.fadeTime = fadeTime;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float start, float end)
    {
        Color color = image.color;
        float current = 0;
        while (current < fadeTime)
        {
            current += Time.deltaTime;
            color.a = Mathf.Lerp(start, end, current / fadeTime);
            image.color = color;
            yield return null;
        }
    }
}
