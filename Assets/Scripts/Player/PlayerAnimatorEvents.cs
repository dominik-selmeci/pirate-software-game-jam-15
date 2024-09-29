using UnityEngine;

public class PlayerAnimatorEvents : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.IsName("Player_aiming") || animatorStateInfo.IsName("Player_aiming_flipped"))
        {
            Player player = animator.GetComponent<Player>();
            player.RestoreMovement();
        }
    }
}