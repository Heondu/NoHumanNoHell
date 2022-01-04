using UnityEngine;

public class TextBubbleCreator : MonoBehaviour
{
    [SerializeField] private TextBubble bubblePrefab;
    private TextBubble textBubble;

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
