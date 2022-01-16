using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashTime;
    private Shader originShader;
    private Shader flashShader;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        originShader = spriteRenderer.material.shader;
        flashShader = Shader.Find("Custom/FlashWhite");
    }

    public void StartAnimation()
    {
        StopCoroutine("Flash");
        StartCoroutine("Flash");
    }

    private IEnumerator Flash()
    {
        spriteRenderer.material.shader = flashShader;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.material.shader = originShader;
    }
}
