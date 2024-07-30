using UnityEngine;

public class Artifact : MonoBehaviour
{
	public GameObject EndGamePanel;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			EndGamePanel.SetActive(true);
			Destroy(GetComponent<SpriteRenderer>());
		}
	}
}
