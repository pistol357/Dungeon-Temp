using UnityEngine;

public class MonsterPlayCard : PlayCard
{
    [SerializeField] private MonsterCard _monsterCardData;

    public MonsterCard MonsterCardData => _monsterCardData;

    public MonsterPlayCard(CardState state, MonsterCard data)
    {
        _cardState = state;
        _monsterCardData = data;
    }
}
