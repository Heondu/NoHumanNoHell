using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleCreator : MonoBehaviour
{
    [SerializeField] private TextBubble bubblePrefab;
    private TextBubble textBubble;

    public void ShowBubble(string message, Transform target)
    {
        if (textBubble != null)
        {
            Destroy(textBubble.gameObject);
        }

        textBubble = Instantiate(bubblePrefab, transform);
        textBubble.Setup(message, target);
    }
}
