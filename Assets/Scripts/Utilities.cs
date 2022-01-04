using UnityEngine;

/// <summary>
/// 개발 시 필요한 기능들을 사용하기 편하게 함수 형태로 묶어 둔 클래스
/// </summary>
public static class Utilities
{
    /// <summary>
    /// 마우스의 월드 좌표를 반환하는 함수
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
