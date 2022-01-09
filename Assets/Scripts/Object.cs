using UnityEngine;
using TMPro;

public enum MessageTriggerType
{
    Start,
    Enter,
    Interact
}

public class Object : MonoBehaviour
{
    [SerializeField] private MessageTriggerType messageTriggerType;
    private Message message;
    private TextMeshPro text;
    private bool isPlayerEnter = false;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        message = GetComponent<Message>();
        text = GetComponentInChildren<TextMeshPro>();
        text.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (messageTriggerType == MessageTriggerType.Start)
            message.ShowMessage();
    }

    private void Update()
    {
        MessageUpdate();
    }

    private void MessageUpdate()
    {
        if (messageTriggerType == MessageTriggerType.Interact && isPlayerEnter && Input.GetKeyDown(KeyCode.E))
            message.ShowMessage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SetMessageVariables();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            ResetMessageVariables();
    }

    private void SetMessageVariables()
    {
        if (messageTriggerType == MessageTriggerType.Enter)
            message.ShowMessage();
        else if (messageTriggerType == MessageTriggerType.Interact)
            text.gameObject.SetActive(true);
        isPlayerEnter = true;
    }

    private void ResetMessageVariables()
    {
        if (messageTriggerType == MessageTriggerType.Interact)
            text.gameObject.SetActive(false);

        isPlayerEnter = false;
        message.ResetIndex();
    }
}
