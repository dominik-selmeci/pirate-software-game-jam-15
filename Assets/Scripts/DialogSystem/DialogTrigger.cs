using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

	public Dialog _dialog;
	public bool playOnce;

	public void TriggerDialogue()
	{
		FindFirstObjectByType<DialogManager>().StartDialog(_dialog);
	}

	public void TriggerEndDialog()
	{
		DialogManager dialogManager = FindFirstObjectByType<DialogManager>();
		if (dialogManager != null && playOnce)
		{
			dialogManager.EndDialog();
			Destroy(gameObject);
		}
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision != null && collision.CompareTag("Player"))
		{
			TriggerDialogue();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision != null && collision.CompareTag("Player"))
		{
			TriggerEndDialog();
		}
	}
}
