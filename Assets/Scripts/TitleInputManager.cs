using UnityEngine;

/// <summary>
/// Ÿ��Ʋ ������ ���� �Է��� üũ�ϴ� Ŭ����
/// </summary>
public class TitleInputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            FindObjectOfType<SceneLoader>().LoadScene();
        }
    }
}
