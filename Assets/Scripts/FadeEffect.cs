using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeTime;
    public UnityEvent onFadeEnded = new UnityEvent();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fade()
    {
        StopCoroutine("FadeCo");
        StartCoroutine("FadeCo");
    }

    private IEnumerator FadeCo()
    {
        float current = 0;
        while (current < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(Color.white, Color.clear, current / fadeTime);
            current += Time.deltaTime;
            yield return null;
        }
        onFadeEnded.Invoke();
    }
}
