using System.Collections.Generic;
using UnityEngine;

public abstract class PlayCard
{
    [SerializeField] protected CardState _cardState;

    protected List<List<ActionType>> _cardAction = new();

    public CardState CardState => _cardState;
    public int CardActionCount => _cardAction.Count;

    public void SetCardState(CardState state)
    {
        _cardState = state;
    }

    public abstract List<ActionType> GetActionList(int num);
}
