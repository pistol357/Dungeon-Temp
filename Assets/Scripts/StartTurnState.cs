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
        if(_turnManager.CurrentRound == 0)
        {
            StartRound();
        }
    }

    public void Exit()
    {

    }

    private void StartRound()
    {
        _currentClan.InitSummon();
    }
}
