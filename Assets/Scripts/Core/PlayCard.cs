using System.Collections.Generic;
using UnityEngine;

public abstract class PlayCard
{
    protected List<List<ActionType>> _cardAction = new();

    public int CardActionCount => _cardAction.Count;
    public int GetActionListsCount => _cardAction.Count;

    public abstract List<ActionType> GetActionList(int num);

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
