public class HeroInvasionState : IState
{
    private TurnManager _turnManager;
    private Guild _guild;

    public HeroInvasionState(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }

    public void Enter()
    {
        _guild = _turnManager.Guild;

        if (_turnManager.CurrentRound == 0)
        {
            _turnManager.StateMachine.ChangeState(_turnManager.EndTurnState);
        }
        else
        {
            _guild.ChangeGuildState(GuildState.Start);
        }
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _guild.ChangeGuildState(GuildState.None);
    }
}
