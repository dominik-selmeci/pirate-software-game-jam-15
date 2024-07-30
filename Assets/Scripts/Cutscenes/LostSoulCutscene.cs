using UnityEngine;

public class LostSoulCutscene : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Animator animator;
    [SerializeField] DialogTrigger dialogTrigger;

	private void Awake()
	{
        if (player != null && animator != null)
        {
            player._canMove = false;
            animator.SetTrigger("Play");
        }
    }

	public void TriggerDialog()
	{
        dialogTrigger.TriggerDialogue();
	}

    public void EndCutscene()
	{
        player._canMove = true;

        Destroy(gameObject);
    }
}
