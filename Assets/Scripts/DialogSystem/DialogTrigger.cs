using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

	public Dialog dialog;

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogManager>().StartDialog(dialog);
	}

	public void TriggerEndDialog()
	{
		FindObjectOfType<DialogManager>().EndDialog();

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
