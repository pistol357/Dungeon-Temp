using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListView : MonoBehaviour
{
    [SerializeField] private ActionListSlot _actionListPrefab;
    [SerializeField] private Transform _ationListSlot;

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        if (_clan.State != WaitFor.SelectActionList)
        {
            _ationListSlot.gameObject.SetActive(false);
            return;
        }
        else if (_clan.State == WaitFor.SelectActionList)
        {
            _ationListSlot.gameObject.SetActive(true);
        }

        foreach (Transform child in _ationListSlot)
        {
            Destroy(child.gameObject);
        }

        PlayCard card = _clan.SelectedCard;
        for (int i = 0; i < card.GetActionListsCount; i++)
        {
            ActionListSlot actionList = Instantiate(_actionListPrefab, _ationListSlot);
            actionList.SetActionListData(card.GetActionList(i));

            if (i != _clan.CurrentIndex)
            {
                actionList.Unselect();
            }
        }
    }
}
