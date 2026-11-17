using UnityEngine;

public class StartTurnState : IState
{
    private TurnManager _turnManager;
    private Clan _currentClan;

    public StartTurnState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        _currentClan = _turnManager.CurrentTurnClan;
        Debug.Log($"{_currentClan} turn start");
    }

    public void Execute()
    {
        _turnManager.StateMachine.ChangeState(_turnManager.MonsterActionState);
    }

    public void Exit()
    {

    }
}
