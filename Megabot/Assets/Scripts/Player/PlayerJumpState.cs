using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player)
    {
        this.player = player;
    }
    public override void EnterState()
    {
        player.animator.Play("Jump");
        player.currentState = PlayerStates.Jump;
        player.rb2D.velocity = new Vector2(player.rb2D.velocity.x, player.stats.jumpForce);
        player.isGrounded = false;
        player.isOnOneWayGrounded = false;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        player.InOneWayGroundCheck();
        
        if (player.rb2D.velocity.y <= 0)
        {
            player.stateMachine.ChangeState(player.stateMachine.fallState);
        }
        else if (player.canDoubleJump && Input.GetButtonDown("Jump"))
        {
            player.stateMachine.ChangeState(player.stateMachine.doubleJumpState);
        }
    }
}
