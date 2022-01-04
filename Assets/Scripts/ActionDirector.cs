using System.Collections;
using UnityEngine;

public class ActionDirector : MonoBehaviour
{
    private CameraController cameraController;
    private Coroutine slowCoroutine;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void StartWeekAttackAction()
    {
        cameraController.Shake(0.05f, 0.05f);
    }

    public void StartStrongAttackAction()
    {
        cameraController.Shake(0.1f, 0.1f);
        Slow(0.4f, 0.2f);
    }

    private void Slow(float timeScale, float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowCo(timeScale, duration));
    }

    private IEnumerator SlowCo(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        float current = 0;
        while (current < duration)
        {
            current += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(timeScale, 1, current / duration);
            yield return null;
        }
        Time.timeScale = 1;
    }
}
