using UnityEngine;

/// <summary>
/// �÷��̾�� ���� ������ �� �������� �̿��ϱ� ���� �������̽�
/// </summary>
public interface ILivingEntity
{
    /// <summary>
    /// ���� ó�� �Լ�
    /// </summary>
    /// <param name="damage">������</param>
    /// <param name="instigator">������ ������Ʈ</param>
    void TakeDamage(int damage, GameObject instigator);
}
