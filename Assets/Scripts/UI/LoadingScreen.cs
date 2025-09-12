using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    private float minLoadTime = 3f;
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
        float timer = 0f;
        while (true)
        {
            float currentProgress = Mathf.Max(operation.progress / 0.9f, timer / minLoadTime);
            progressBar.value = currentProgress;

            if (operation.progress >= 0.9f && timer >= minLoadTime)
            {
                operation.allowSceneActivation = true;
                break; 
            }

            timer += Time.deltaTime;

            yield return null;
        }
        progressBar.value = 1f;
    }
}
