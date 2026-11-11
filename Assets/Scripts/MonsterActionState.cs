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
        _currentClan.WaitForInput(true);
        _currentClan.ChangeState(WaitFor.SelectCard);
    }

    public void Execute()
    {
        if (_turnManager.CurrentRound == 0)
        {
            StartRound();
        }
    }

    public void Exit()
    {
        _currentClan.WaitForInput(false);
        _turnManager.EndMonsterActionState();
    }

    private void StartRound()
    {
        _currentClan.InitSummon();
    }
}
