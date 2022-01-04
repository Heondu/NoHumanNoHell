using System.Collections;
using UnityEngine;

public class ColorFlashEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashTime;
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.white;

    public void StartAnimation()
    {
        StopCoroutine("Flash");
        StartCoroutine("Flash");
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = startColor;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = endColor;
    }
}
