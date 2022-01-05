using UnityEngine;

public class TextBubbleCreator : MonoBehaviour
{
    [SerializeField] private TextBubble bubblePrefab;
    private TextBubble textBubble;

    public void ShowBubble(string message, Vector2 position)
    {
        if (textBubble != null)
        {
            Destroy(textBubble.gameObject);
        }

        textBubble = Instantiate(bubblePrefab, transform);
        textBubble.Setup(message, position);
    }
}
