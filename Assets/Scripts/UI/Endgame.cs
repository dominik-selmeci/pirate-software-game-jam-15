using UnityEngine;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
	public GameObject gameOverUI;

	public void Restart()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}
}
