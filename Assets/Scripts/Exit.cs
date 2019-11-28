 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
	private int currentSceneIndex;
    public static bool levelComplete = false;
	void Start()
    {
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			StartCoroutine(LoadNextScene());
		}
	}

	IEnumerator LoadNextScene()
	{
        levelComplete = true;
		Initiate.Fade("", Color.black, 1);
		yield return new WaitForSeconds(1);
        SceneManager.LoadScene(++currentSceneIndex);
        levelComplete = false;
    }
}
