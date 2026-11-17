using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayCard
{
    [SerializeField] private PlayCardData _cardData;
    [SerializeField] private CardType _cardType;

    private List<List<ActionType>> _cardAction = new();

    public PlayCardData CardData => _cardData;
    public int CardActionCount => _cardAction.Count;
    public int GetActionListsCount => _cardAction.Count;

    public PlayCard(PlayCardData data)
    {
        _cardData = data;
        _cardType = data.CardType;
        if (data.ActionList1 != null && data.ActionList1.Count != 0) _cardAction.Add(data.ActionList1);
        if (data.ActionList2 != null && data.ActionList2.Count != 0) _cardAction.Add(data.ActionList2);
        if (data.ActionList3 != null && data.ActionList3.Count != 0) _cardAction.Add(data.ActionList3);
    }

    public List<ActionType> GetActionList(int num)
    {
        return _cardAction[num].ToList();
    }

    public string PrintCardInfo()
    {
        string info = "현재 카드 : ";
        for (int i = 0; i < GetActionListsCount; i++)
        {
            foreach (ActionType action in GetActionList(i))
            {
                info += $"{action} ";
            }
            info += "/ ";
        }

        Debug.Log(info);
        return info;
    }

    public string PrintActionListInfo(int num)
    {
        string info = "현재 효과 : ";
        foreach (ActionType action in GetActionList(num))
        {
            info += $"{action} ";
        }

        Debug.Log(info);
        return info;
    }
}
