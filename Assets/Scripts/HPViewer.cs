using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum HPViewerType
{
    Player,
    Boss
}

public class HPViewer : MonoBehaviour
{
    [SerializeField] private Image slider;
    [SerializeField] private TextMeshProUGUI text;

    [Header("HP Viewer Type")]
    [SerializeField] private HPViewerType type;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        GameObject.FindGameObjectWithTag(type.ToString()).GetComponent<Entity>().Status.onModifierUpdate.AddListener(UpdateView);
    }

    private void UpdateView(Status status, StatusType statusType, float currentValue, float pervValue)
    {

        if (statusType == StatusType.CurrentHP)
        {
            float maxValue = status.GetValue(StatusType.MaxHP);
            slider.fillAmount = currentValue / maxValue;
            text.text = $"{currentValue}/{maxValue}";
        }
    }
}
