using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private CardView _cardPrefab;
    [SerializeField] private Transform _cardsListView;

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        if (_clan.State != WaitFor.SelectCard)
        {
            _cardsListView.gameObject.SetActive(false);
            return;
        }
        else if (_clan.State == WaitFor.SelectCard)
        {
            _cardsListView.gameObject.SetActive(true);
        }

        foreach(Transform child in _cardsListView)
        {
            Destroy(child.gameObject);
        }

        List<PlayCard> hand = _clan.DeckManager.Hand;
        for(int i = 0; i < hand.Count; i++)
        {
            CardView card = Instantiate(_cardPrefab, _cardsListView);
            card.SetCardData(hand[i].CardData);

            if (i == _clan.CurrentIndex)
            {
                card.Select();
            }
        }
    }
}
