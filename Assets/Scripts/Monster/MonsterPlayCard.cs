using System.Collections.Generic;
using UnityEngine;

public class MonsterPlayCard : PlayCard
{
    [SerializeField] private MonsterCardData _monsterCardData;

    public MonsterCardData MonsterCardData => _monsterCardData;

    public MonsterPlayCard(MonsterCardData data)
    {
        _monsterCardData = data;
        _cardAction?.Add(data.ActionList1);
        _cardAction?.Add(data.ActionList2);
    }

    public override List<ActionType> GetActionList(int num)
    {
        return _cardAction[num];
    }
}
