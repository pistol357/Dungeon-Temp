using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private List<Clan> _clans;

    private int _currentRound;
    private int _currentTurnIndex;
    private Clan _currentTurnClan;

    public static TurnManager Instance { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public StartTurnState StartTurnState { get; private set; }
    public EndTurnState EndTurnState { get; private set; }
    public event Action OnEndTurn;
    public int CurrentRound => _currentRound;
    public int CurrentTurnIndex => _currentTurnIndex;
    public Clan CurrentTurnClan => _currentTurnClan;

    private void Awake()
    {
        SetSingleton();
        CreateStates();
    }
    private void Start() => Init();
    private void Update() => StateMachine.Execute();

    public void NextClan()
    {
        _currentTurnIndex++;

        if(_currentTurnIndex == _clans.Count)
        {
            _currentRound++;
            _currentTurnIndex %= _clans.Count;
        }

        _currentTurnClan = _clans[_currentTurnIndex];
    }

    private void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void CreateStates()
    {
        StateMachine = new StateMachine();
        StartTurnState = new StartTurnState(this);
        EndTurnState = new EndTurnState(this);
    }

    private void Init()
    {
        _currentTurnClan = _clans[_currentTurnIndex];
        StateMachine.Initialize(StartTurnState);
    }
}
