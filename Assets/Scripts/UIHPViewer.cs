using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHPViewer : MonoBehaviour
{
    [SerializeField] private Status targetStatus;
    private Slider slider;
    private TextMeshProUGUI text;

    private void Start()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        slider.value = targetStatus.GetCurrentHP() / targetStatus.GetMaxHP();
        text.text = $"{targetStatus.GetCurrentHP()}/{targetStatus.GetMaxHP()}";
    }
}
