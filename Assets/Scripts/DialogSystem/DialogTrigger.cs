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
}
