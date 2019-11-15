using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
	private int currentSceneIndex;

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
		Initiate.Fade("", Color.black, 1.5f);
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(++currentSceneIndex);
	}
}
