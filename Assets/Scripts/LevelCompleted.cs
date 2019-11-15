using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
	private int CurrentScene;
	private Color loadToColor = Color.black;
	void Start()
    {
		CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		print(col.gameObject.name);
		if (col.gameObject.name == "Player")
		{
			StartCoroutine("LoadNextLevel");
		}
	}

	IEnumerator LoadNextLevel()
	{// fade out and go to the next level
		Initiate.Fade("", loadToColor, 1.5f);
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(++CurrentScene);
	}
}
