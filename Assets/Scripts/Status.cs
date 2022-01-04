using UnityEngine;

/// <summary>
/// 오브젝트의 체력을 관리하는 클래스
/// </summary>
public class Status : MonoBehaviour
{
    [SerializeField] private int maxHP;
    private int currentHP;
    [SerializeField] private bool canBeDamaged = true;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (!canBeDamaged) return;

        currentHP = Mathf.Max(0, currentHP - amount);
    }

    public void RestoreHP(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }
}
