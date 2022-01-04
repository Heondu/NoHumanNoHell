using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬을 전환하는 클래스
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
