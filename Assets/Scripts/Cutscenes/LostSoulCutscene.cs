using UnityEngine;

public class LostSoulCutscene : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Animator animator;
    [SerializeField] DialogTrigger dialogTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
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
