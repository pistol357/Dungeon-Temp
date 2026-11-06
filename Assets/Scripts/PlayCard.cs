using UnityEngine;

public abstract class PlayCard
{
    [SerializeField] protected CardState _cardState;

    public CardState CardState => _cardState;

    public void SetCardState(CardState state)
    {
        _cardState = state;
    }
}
