using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TextPrinter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string id;
    [SerializeField] private float interval;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private UnityEvent onPrintDone = new UnityEvent();
    private List<string> messages = new List<string>();
    private int currentLine = 0;
    private bool isDone = false;

    private void Start()
    {
        messages = DBManager.Instance.FindMessages(id);

        StartCoroutine("TypeWriter");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!isDone)
            {
                isDone = true;
                StopCoroutine("TypeWriter");
                text.text = messages[currentLine];
                return;
            }
            if (currentLine < messages.Count - 1)
            {
                currentLine++;
                StartCoroutine("TypeWriter");
            }
            else
            {
                onPrintDone.Invoke();
                enabled = false;
            }
        }
    }

    private IEnumerator TypeWriter()
    {
        isDone = false;
        text.text = "";

        for (int i = 0; i < messages[currentLine].Length; i++)
        {
            SoundManager.SoundAllMute();
            SoundManager.PlaySFX(clips[Random.Range(0, clips.Length)]);
            text.text += messages[currentLine][i];

            yield return new WaitForSeconds(interval);
        }

        isDone = true;
    }
}
