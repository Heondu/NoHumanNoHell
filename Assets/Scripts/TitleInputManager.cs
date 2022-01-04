using UnityEngine;

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
