using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AudioClip OnClickSound;

    public void OnClick()
    {
        StartCoroutine(LoadFirstScene());
    }
    public void OnExit()
    {
        Application.Quit();
    }

    IEnumerator LoadFirstScene()
        {
            // Fade to black
            Initiate.Fade("", Color.black, 1);

            // Play click sound
            GetComponent<AudioSource>().PlayOneShot(OnClickSound);

            // Wait a second
            yield return new WaitForSeconds(1);

            // Load the first scene
            SceneManager.LoadScene(1);
        }
}
