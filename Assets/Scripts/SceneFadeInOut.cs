using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 씬 전환시 부드럽게 전환을 위해 Fade In/Out을 제어하는 클래스
/// </summary>
public class SceneFadeInOut : MonoBehaviour
{
    private Image image;
    private Coroutine fadeCoroutine;
    private float fadeTime;

    private void Start()
    {
        image = GetComponent<Image>();

        //씬 시작 시 페이드 아웃
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
        float percent = 0;
        while (current < fadeTime)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;
            yield return null;
        }
    }
}
