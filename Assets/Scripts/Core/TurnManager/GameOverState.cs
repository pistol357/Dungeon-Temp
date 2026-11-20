using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(3);
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
