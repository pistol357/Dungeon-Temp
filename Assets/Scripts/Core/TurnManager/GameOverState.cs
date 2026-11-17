using UnityEngine;

public class GameOverState : IState
{
    private TurnManager _turnManager;

    public GameOverState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        Debug.Log($"GAME OVER");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
