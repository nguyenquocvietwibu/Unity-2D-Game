using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerDoubleJumpState : PlayerState
{
    public PlayerDoubleJumpState(Player player)
    {
        this.player = player;
    }
    public override void EnterState()
    {
        player.animator.Play("Double Jump");
        player.currentState = PlayerStates.DoubleJump;
        player.rb2D.velocity = new Vector2(player.rb2D.velocity.x, player.stats.jumpForce / 1.5f);
        player.isGrounded = false;
        player.canDoubleJump = false;
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
    }
}
