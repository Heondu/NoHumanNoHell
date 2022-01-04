using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 플레이어와 상호작용하며 대사를 출력하는 클래스
/// </summary>
public class Object : MonoBehaviour
{
    [SerializeField] private string id;
    private TextMeshPro text;
    private List<string> messages;
    private int currentIndex = 0;
    private bool isPlayerEnter = false;
    private TextBubbleCreator textBubbleCreator;
    [SerializeField] private Vector3 textOffset;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.gameObject.SetActive(false);
        textBubbleCreator = FindObjectOfType<TextBubbleCreator>();

        messages = DBManager.Instance.FindMessages(id);
    }

    private void Update()
    {
        if (!isPlayerEnter) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentIndex < messages.Count)
            {
                textBubbleCreator.ShowBubble(messages[currentIndex], transform.position + textOffset);
                currentIndex++;

                if (currentIndex >= messages.Count)
                {
                    currentIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = false;
            text.gameObject.SetActive(false);
            currentIndex = 0;
        }
    }
}
