using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player)
    {
        this.player = player;
    }
    public override void EnterState()
    {
        player.animator.Play("Fall");
        player.currentState = PlayerStates.Fall;
        player.isGrounded = false;
        player.isOnOneWayGrounded = false;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        player.InOneWayGroundCheck();
        player.GroundCheck();
        if (Input.GetButtonDown("Jump") && player.canDoubleJump)
        {
            player.stateMachine.ChangeState(player.stateMachine.doubleJumpState);
        }
        if ((player.isGrounded || player.isOnOneWayGrounded) && !player.isInOneWayGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.idleState);
        }
    }

}
