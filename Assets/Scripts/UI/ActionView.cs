using System.Collections.Generic;
using UnityEngine;

public class ActionView : MonoBehaviour
{
    [SerializeField] private GameObject _actionListPrefab;
    [SerializeField] private GameObject _actionIconPrefab;
    [SerializeField] private Transform _actionSlot;

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        foreach (Transform child in _actionSlot)
        {
            Destroy(child.gameObject);
        }

        if(_clan.SelectedAction != ActionType.None)
        {
            ActionIconView selectedActionIcon = Instantiate(_actionIconPrefab, _actionSlot).GetComponent<ActionIconView>();
            selectedActionIcon.SetIcon(UIManager.Instance.GetSpriteByActionType(_clan.SelectedAction));
        }

        if(_clan.State == WaitFor.SelectActionList)
        {
            PlayCard card = _clan.SelectedCard;
            for (int i = 0; i < card.GetActionListsCount; i++)
            {
                ActionListView actionList = Instantiate(_actionListPrefab, _actionSlot).GetComponent<ActionListView>();
                actionList.SetActionList(card.GetActionList(i));

                if (i != _clan.CurrentIndex)
                {
                    actionList.Unselect();
                }
            }
        }

        if (_clan.State == WaitFor.SelectAction)
        {
            List<ActionType> actionList = _clan.SelectedActionList;
            for(int i = 0; i < actionList.Count; i++)
            {
                ActionIconView actionIcon = Instantiate(_actionIconPrefab, _actionSlot).GetComponent<ActionIconView>();
                actionIcon.SetIcon(UIManager.Instance.GetSpriteByActionType(actionList[i]));

                if (i == _clan.CurrentIndex)
                {
                    actionIcon.Select();
                }
                else if (i != _clan.CurrentIndex)
                {
                    actionIcon.Unselect();
                }
            }
        }
    }
}
