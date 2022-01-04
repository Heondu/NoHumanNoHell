using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        EventSetup();
    }

    private void EventSetup()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Status.onModifierUpdate.AddListener(UpdateView);
    }

    private void UpdateView(Status status, StatusType statusType, float currentValue, float pervValue)
    {
        if (statusType == StatusType.CurrentHP)
        {
            float maxValue = status.GetValue(StatusType.MaxHP);
            slider.value = currentValue / maxValue;
            text.text = $"{currentValue}/{maxValue}";
        }
    }
}
