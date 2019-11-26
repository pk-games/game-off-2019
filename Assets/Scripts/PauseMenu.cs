using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static bool gameIsPaused;
    public GameObject overlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
		}
    }

    public void Resume()
    {
        overlay.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
        Resume();
    }

    void Pause()
    {
        overlay.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }
}
