using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ��ȯ�ϴ� Ŭ����
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
