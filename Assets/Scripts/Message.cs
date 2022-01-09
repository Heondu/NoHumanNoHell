using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private Vector3 textOffset;

    private TextBubbleCreator textBubbleCreator;
    
    private List<string> messages;
    private int currentIndex = 0;

    private void Start()
    {
        textBubbleCreator = FindObjectOfType<TextBubbleCreator>();
        GetMessagesFromDB();
    }

    private void GetMessagesFromDB()
    {
        messages = DBManager.Instance.FindMessages(id);
    }

    public void ShowMessage()
    {
        if (currentIndex >= messages.Count)
            currentIndex = 0;
        
        textBubbleCreator.ShowBubble(messages[currentIndex], transform.position + textOffset);
        currentIndex++;
    }

    public void ResetIndex()
    {
        currentIndex = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + textOffset, Vector3.one * 0.1f);
    }
}
