using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListView : MonoBehaviour
{
    [SerializeField] private Image _selectActionList;
    [SerializeField] private GameObject _actionIconPrefab;
    [SerializeField] private Transform _actionListSlot;

    public void SetActionList(List<ActionType> actionList)
    {
        if (actionList != null && actionList.Count > 0)
        {
            foreach (ActionType action in actionList)
            {
                ActionIconView actionIcon = Instantiate(_actionIconPrefab, _actionListSlot).GetComponent<ActionIconView>();
                actionIcon.SetIcon(UIManager.Instance.GetSpriteByActionType(action));
                actionIcon.Unselect();
            }
        }
    }

    public void Unselect()
    {
        _selectActionList.GetComponent<Image>().enabled = false;
    }
}
