using UnityEngine;

/// <summary>
/// ��� UI�� �����ϰ� �����ϴ� Ŭ����
/// </summary>
public class TextBubbleCreator : MonoBehaviour
{
    [SerializeField] private TextBubble bubblePrefab;
    private TextBubble textBubble;

    /// <summary>
    /// ��� UI ���� �Լ�
    /// </summary>
    /// <param name="message"></param>
    /// <param name="position">���� ��ġ</param>
    public void ShowBubble(string message, Vector2 position)
    {
        //���� ��� UI�� ���� ��� ����
        if (textBubble != null)
        {
            Destroy(textBubble.gameObject);
        }

        textBubble = Instantiate(bubblePrefab, transform);
        textBubble.Setup(message, position);
    }
}
