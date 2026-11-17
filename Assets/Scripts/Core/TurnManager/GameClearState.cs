using UnityEngine;

public class GameClearState : IState
{
    private TurnManager _turnManager;

    public GameClearState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        Debug.Log($"GAME CLEAR");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
