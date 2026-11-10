public class StateMachine
{
    private IState _currentState;

    public IState CurrentState => _currentState;

    public void Initialize(IState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(IState nextState)
    {
        _currentState.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void Execute()
    {
        _currentState.Execute();
    }
}
