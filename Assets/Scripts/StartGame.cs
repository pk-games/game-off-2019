using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AudioClip OnClickSound;

    private int firstSceneIndex = 1;

    public void OnClick()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        // Fade to black
        Initiate.Fade("", Color.black, 1);

        // Play click sound
        GetComponent<AudioSource>().PlayOneShot(OnClickSound);

        // Wait a second
        yield return new WaitForSeconds(1);

        // Load the first scene
        SceneManager.LoadScene(firstSceneIndex);
    }
}
