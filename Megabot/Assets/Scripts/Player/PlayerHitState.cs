using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    AnimatorStateInfo stateInfo;
    public PlayerHitState(Player player)
    {
        this.player = player;
    }
    public override void EnterState()
    {
        player.animator.Play("Hit");
        player.currentState = PlayerStates.Hit;
        player.canMove = false;
        stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
    }

    public override void ExitState()
    {
        player.canMove = true;
    }

    public override void UpdateState()
    {
        if (stateInfo.normalizedTime >= 1f && !player.animator.IsInTransition(0))
        {
            player.stateMachine.ChangeState(player.stateMachine.idleState);
        }
    }
}
