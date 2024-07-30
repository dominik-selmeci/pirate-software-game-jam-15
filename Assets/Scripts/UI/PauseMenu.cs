using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;
	DamagePanel damagePanel;

	private void Start()
	{
		damagePanel = GetComponentInParent<DamagePanel>();
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //TODO: change this
		{
			if (GameIsPaused)
			{
				if (damagePanel != null)
				{
					damagePanel.DisabelPanel();
				}
				ResumeGame();
			}else
			{
				if (damagePanel != null)
				{
					damagePanel.DisabelPanel();
				}
				PauseGame();
			}
		}
    }

	public void ResumeGame()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void PauseGame()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
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
