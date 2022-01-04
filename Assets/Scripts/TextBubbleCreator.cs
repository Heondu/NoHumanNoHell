using UnityEngine;

/// <summary>
/// 대사 UI를 생성하고 삭제하는 클래스
/// </summary>
public class TextBubbleCreator : MonoBehaviour
{
    [SerializeField] private TextBubble bubblePrefab;
    private TextBubble textBubble;

    /// <summary>
    /// 대사 UI 생성 함수
    /// </summary>
    /// <param name="message"></param>
    /// <param name="position">생성 위치</param>
    public void ShowBubble(string message, Vector2 position)
    {
        //이전 대사 UI가 있을 경우 삭제
        if (textBubble != null)
        {
            Destroy(textBubble.gameObject);
        }

        textBubble = Instantiate(bubblePrefab, transform);
        textBubble.Setup(message, position);
    }
}
