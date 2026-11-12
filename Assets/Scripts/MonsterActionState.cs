using UnityEngine;

public class MonsterActionState : IState
{
    private TurnManager _turnManager;
    private Clan _currentClan;

    public MonsterActionState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        _currentClan = _turnManager.CurrentTurnClan;

        if (_turnManager.CurrentRound == 0)
        {
            StartRound();
        }
        else
        {
            _currentClan.ChangeState(WaitFor.SelectCard);
        }
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _currentClan.ChangeState(WaitFor.None);
        _turnManager.EndMonsterActionState();
    }

    private void StartRound()
    {
        _currentClan.InitSummon();
    }
}
