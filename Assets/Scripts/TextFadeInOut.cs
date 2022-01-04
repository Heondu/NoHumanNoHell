using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// 텍스트의 Fade In/Out 효과를 제어하는 클래스
/// </summary>
public class TextFadeInOut : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine("FadeUpdate");
    }

    private IEnumerator FadeUpdate()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));
            yield return StartCoroutine(Fade(0, 1));
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        Color color = text.color;
        float current = 0;
        float percent = 0;
        while (current < fadeTime)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;
            yield return null;
        }
    }
}
