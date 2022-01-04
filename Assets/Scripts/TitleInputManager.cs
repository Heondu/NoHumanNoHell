using UnityEngine;

/// <summary>
/// 타이틀 씬에서 유저 입력을 체크하는 클래스
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
