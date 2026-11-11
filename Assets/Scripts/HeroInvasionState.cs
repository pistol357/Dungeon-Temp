using UnityEngine;

public class HeroInvasionState : IState
{
    private TurnManager _turnManager;
    private Clan _currentClan;

    public HeroInvasionState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        _currentClan = _turnManager.CurrentTurnClan;
    }

    public void Execute()
    {
        _turnManager.StateMachine.ChangeState(_turnManager.EndTurnState);
    }

    public void Exit()
    {

    }
}
