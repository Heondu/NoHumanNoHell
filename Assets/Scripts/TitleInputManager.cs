using UnityEngine;

public class TitleInputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return;
        if (GameManager.Instance.IsStop)
            return;

        if (Input.anyKeyDown)
            FindObjectOfType<SceneLoader>().LoadScene();
    }
}
