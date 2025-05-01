using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player)
    {
        this.player = player;
    }
    public override void EnterState()
    {
        player.animator.Play("Run");
        player.currentState = PlayerStates.Run;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        player.InOneWayGroundCheck();
        player.GroundCheck();
        if (player.isGrounded || player.isOnOneWayGrounded)
        {
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Input.GetButtonDown("Jump") && player.isOnOneWayGrounded)
            {
                player.coroutine = player.StartCoroutine(player.DisableCollision(0.25f));
                player.stateMachine.ChangeState(player.stateMachine.fallState);
            }
            else if (Input.GetButtonDown("Jump"))
            {
                player.stateMachine.ChangeState(player.stateMachine.jumpState);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 && player.canMove)
            {
                player.rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * player.stats.moveSpeed, player.rb2D.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                player.stateMachine.ChangeState(player.stateMachine.idleState);
            }
            
        }
        else
        {
            player.stateMachine.ChangeState(player.stateMachine.fallState);
        }
    }
}
