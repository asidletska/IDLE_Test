using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    private void Start()
    {
        SceneLoad("SampleScene");
    }
    public void SceneLoad(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        float progress = 0;
        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            progress = Mathf.MoveTowards(progress, targetProgress, Time.deltaTime);
            progressBar.value = progress;
            if (progress >= 1f && operation.progress >= 0.9f)
            {
                // Активуємо сцену
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
