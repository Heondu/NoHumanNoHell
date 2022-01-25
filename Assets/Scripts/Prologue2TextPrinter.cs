using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prologue2TextPrinter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string id;
    [SerializeField] private float interval;
    private List<string> messages = new List<string>();
    private int currentLine = 0;

    private void Start()
    {
        messages = DBManager.Instance.FindMessages(id);
    }

    public void Print()
    {
        StartCoroutine("TypeWriter");
    }

    private IEnumerator TypeWriter()
    {
        text.text = "";

        for (int i = 0; i < messages[currentLine].Length; i++)
        {
            text.text += messages[currentLine][i];

            yield return new WaitForSeconds(interval);
        }
        currentLine++;
    }
}
