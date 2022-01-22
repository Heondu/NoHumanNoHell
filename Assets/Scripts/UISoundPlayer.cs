using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.PlaySFX(clip);
        }
    }
}
