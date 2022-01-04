using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// 문자열을 출력하는 클래스
/// </summary>
public class TextBubble : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string message;
    private Vector2 position;
    [SerializeField] private float displayTime;
    [SerializeField] private float interval;

    public void Setup(string message, Vector2 position)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        this.message = message;
        this.position = position;

        StartCoroutine("TypeWriter");
    }

    private IEnumerator TypeWriter()
    {
        text.text = "";

        //지정한 시간 간격에 맞춰 문자열을 하나씩 출력
        for (int i = 0; i < message.Length; i++)
        {
            text.text += message[i];

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(displayTime);

        Destroy(gameObject);
    }

    private void Update()
    {
        SetPosition();
    }

    /// <summary>
    /// UI를 월드 좌표에 위치시키는 함수
    /// </summary>
    private void SetPosition()
    {
        RectTransform CanvasRect = transform.root.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }
}
