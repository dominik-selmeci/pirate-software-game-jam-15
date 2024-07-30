using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	public GameObject gameOverUI;

	public void gameOver()
	{
		gameOverUI.SetActive(true);
		Time.timeScale = 0f;
	}

	public void Restart()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}
}
