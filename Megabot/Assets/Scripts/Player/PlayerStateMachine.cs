using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;
    public PlayerIdleState idleState;
    public PlayerHitState hitState;
    public PlayerFallState fallState;
    public PlayerJumpState jumpState;
    public PlayerRunState runState;
    public PlayerDoubleJumpState doubleJumpState;

    public PlayerStateMachine(Player player)
    {
        idleState = new PlayerIdleState(player);
        hitState = new PlayerHitState(player);
        fallState = new PlayerFallState(player);
        jumpState = new PlayerJumpState(player);
        runState = new PlayerRunState(player);
        doubleJumpState = new PlayerDoubleJumpState(player);
    }

    public void InitializeState(PlayerState initialState)
    {
        if (currentState == null)
        {
            currentState = initialState;
            currentState.EnterState();
        }
        else
        {
            throw new System.Exception("đã có state cài sẵn");
        }
    }

    public void ChangeState(PlayerState changedState)
    {
        if (currentState != changedState)
        {
            currentState.ExitState();
            currentState = changedState;
            currentState.EnterState();
        }
    }

    public void UpdateState()
    {
        currentState.UpdateState();
    }
}

public enum PlayerStates
{
    Idle,
    Hit,
    Fall,
    Jump,
    Run,
    DoubleJump,
}
