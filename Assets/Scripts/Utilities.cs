using UnityEngine;

/// <summary>
/// ���� �� �ʿ��� ��ɵ��� ����ϱ� ���ϰ� �Լ� ���·� ���� �� Ŭ����
/// </summary>
public static class Utilities
{
    /// <summary>
    /// ���콺�� ���� ��ǥ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        return worldPos;
    }
}
