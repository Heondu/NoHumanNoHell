using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 플레이어 체력과 체력바 UI를 연동하는 클래스
/// </summary>
public class UIHPViewer : MonoBehaviour
{
    [SerializeField] private Status targetStatus;
    private Slider slider;
    private TextMeshProUGUI text;

    private void Start()
    {
        targetStatus = FindObjectOfType<PlayerController>().GetComponent<Status>();
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (targetStatus == null) return;

        slider.value = targetStatus.GetCurrentHP() / targetStatus.GetMaxHP();
        text.text = $"{targetStatus.GetCurrentHP()}/{targetStatus.GetMaxHP()}";
    }
}
