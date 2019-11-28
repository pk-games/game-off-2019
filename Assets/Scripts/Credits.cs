using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private void Start()
    {
        MusicManager.instance.PlayCalmMusic();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(LoadMenu());
        }
    }
    IEnumerator LoadMenu()
    {
        Initiate.Fade("", Color.black, 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
