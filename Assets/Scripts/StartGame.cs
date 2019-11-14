using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartGame : MonoBehaviour
{
    public AudioClip OnClickSound;
    private int firstLevel = 1;
    public Color loadToColor = Color.black;


    public void OnClick()
    {
        StartCoroutine("LoadLevel");
    }

    IEnumerator LoadLevel()
    {
        Initiate.Fade("", loadToColor, 1.5f);
        GetComponent<AudioSource>().PlayOneShot(OnClickSound);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(firstLevel);
    }
}
