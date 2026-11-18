using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionView : MonoBehaviour
{
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

        if (_clan.State == WaitFor.SelectAction)
        {
            List<ActionType> actionList = _clan.SelectedActionList;
            for (int i = 0; i < actionList.Count; i++)
            {
                List<Image> actions = Instantiate(_actionIconPrefab, _actionSlot).GetComponentsInChildren<Image>().ToList();
                actions[1].sprite = UIManager.Instance.GetSpriteByActionType(actionList[i]);

                if (i == _clan.CurrentIndex)
                {
                    actions[0].gameObject.SetActive(true);
                }
                else if (i != _clan.CurrentIndex)
                {
                    actions[0].gameObject.SetActive(false);
                }
            }
        }
        else if (_clan.SelectedAction != ActionType.None)
        {
            List<ActionType> actionList = new();
            actionList.Add(_clan.SelectedAction);

            List<Image> actions = Instantiate(_actionIconPrefab, _actionSlot).GetComponentsInChildren<Image>().ToList();
            actions[1].sprite = UIManager.Instance.GetSpriteByActionType(actionList[0]);
        }
    }
}
