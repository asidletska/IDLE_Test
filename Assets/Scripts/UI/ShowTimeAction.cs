using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTimeAction : MonoBehaviour
{
    [SerializeField] private Image circleFill; // Image з Fill Method = Radial 360
    [SerializeField] private float duration = 5f; // час у секундах

    private Coroutine timerRoutine;

    public void StartTimer(float time = -1f)
    {
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        if (time > 0)
            duration = time;

        timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        float elapsed = 0f;
        circleFill.fillAmount = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            circleFill.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        circleFill.fillAmount = 1f; // гарантовано заповниться в кінці
        timerRoutine = null;

        Debug.Log("Таймер завершився!");
    }
}
