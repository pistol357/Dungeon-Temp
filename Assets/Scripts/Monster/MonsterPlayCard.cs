using UnityEngine;

public class MonsterPlayCard : PlayCard
{
    [SerializeField] private MonsterCardData _monsterCardData;

    public MonsterCardData MonsterCardData => _monsterCardData;

    public MonsterPlayCard(CardState state, MonsterCardData data)
    {
        _cardState = state;
        _monsterCardData = data;
    }
}
