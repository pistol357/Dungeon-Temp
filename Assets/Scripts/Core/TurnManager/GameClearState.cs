using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(2);
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
