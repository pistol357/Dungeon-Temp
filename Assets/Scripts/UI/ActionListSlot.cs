using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListSlot : MonoBehaviour
{
    [SerializeField] private Image _selectActionList;
    [SerializeField] private Image _actionIconPrefab;
    [SerializeField] private Transform _actionListSlot;

    public void SetActionListData(List<ActionType> actionList)
    {
        if (actionList != null && actionList.Count > 0)
        {
            foreach (ActionType action in actionList)
            {
                Image actionIcon = Instantiate(_actionIconPrefab, _actionListSlot);
                actionIcon.sprite = UIManager.Instance.GetSpriteByActionType(action);
            }
        }
    }

    public void Unselect()
    {
        _selectActionList.GetComponent<Image>().enabled = false;
    }
}
