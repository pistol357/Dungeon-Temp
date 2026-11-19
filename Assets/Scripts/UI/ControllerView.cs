using System.Collections.Generic;
using UnityEngine;

public class ControllerView : MonoBehaviour
{
    [SerializeField] private GameObject _unselectPrefab;
    [SerializeField] private GameObject _endPrefab;
    [SerializeField] private Transform _UISlot;

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        foreach (Transform child in _UISlot)
        {
            Destroy(child.gameObject);
        }

        if(_clan.State != WaitFor.None)
        {
            if (_clan.State != WaitFor.SelectCard && !_clan.IsActioned)
            {
                Instantiate(_unselectPrefab, _UISlot);
            }

            if (_clan.State == WaitFor.SelectCard || _clan.State == WaitFor.SelectActionList || _clan.State == WaitFor.SelectAction)
            {
                Instantiate(_endPrefab, _UISlot);
            }
        }
    }
}
