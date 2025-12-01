using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void NextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            nextIndex = 0; // wrap to start
        SceneManager.LoadScene(nextIndex);
    }
    public void PreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int prevIndex = currentIndex - 1;
        if (prevIndex < 0)
            prevIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(prevIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadSceneByIndex(int index)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
            return;
        SceneManager.LoadScene(index);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;
        SceneManager.LoadScene(sceneName);
    }
}
