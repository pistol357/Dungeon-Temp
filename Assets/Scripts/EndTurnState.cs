using UnityEngine;

public class EndTurnState : IState
{
    private TurnManager _turnManager;
    private Clan _currentClan;

    public EndTurnState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        _currentClan = _turnManager.CurrentTurnClan;
        Debug.Log($"{_currentClan} turn end");
    }

    public void Execute()
    {
        StartNextTurn();
    }

    public void Exit()
    {

    }

    private void StartNextTurn()
    {
        _turnManager.NextClan();
        _turnManager.StateMachine.ChangeState(_turnManager.StartTurnState);
    }
}
