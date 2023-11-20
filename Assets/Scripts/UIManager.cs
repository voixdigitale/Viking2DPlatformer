using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] private GameObject _pausePanel;
    public bool IsPaused = false;
    public void Pause() {
        IsPaused = true;
        _pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        IsPaused = false;
        _pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}