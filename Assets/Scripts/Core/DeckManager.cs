using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<PlayCard> _deck = new();
    [SerializeField] private List<PlayCard> _hand = new();
    [SerializeField] private List<PlayCard> _open = new();
    [SerializeField] private List<PlayCard> _discard = new();
    [SerializeField] private PlayCardObject _playCardPrefab;

    public int HandCount => _hand.Count;

    private void OnEnable() => BindEndTurnEvent();
    private void OnDisable() => UnbindEndTurnEvent();

    public void DrawCard()
    {
        if (_deck.Count > 0)
        {
            PlayCard card = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(card);

            Debug.Log($"{card} 뽑음");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }
        else if (_deck.Count == 0)
        {
            if (_discard.Count == 0)
            {
                Debug.Log($"뽑을 카드 부족");
                return;
            }

            Shuffle();
            DrawCard();
        }
    }

    public PlayCard GetCard(int num)
    {
        if (_hand.Count <= 0)
        {
            Debug.Log($"손 패 부족");
            return null;
        }

        if (num < 0 || _hand.Count <= num)
        {
            Debug.Log($"해당 카드 없음(인덱스 범위 벗어남)");
            return null;
        }

        return _hand[num];
    }

    public PlayCard SelectCard(int num)
    {
        PlayCard card = GetCard(num);
        
        if(card != null)
        {
            _hand.Remove(card);
            _open.Add(card);

            Debug.Log($"{card.PrintCardInfo()} 선택됨");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }

        return card;
    }

    public void UnselectCard(PlayCard card)
    {
        if (_open[_open.Count - 1] == card)
        {
            _open.Remove(card);
            _hand.Add(card);

            Debug.Log($"{card.PrintCardInfo()} 선택 취소");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }
    }

    public void EndMonsterActionState()
    {
        foreach (PlayCard card in _open)
        {
            _discard.Add(card);
        }

        _open.Clear();

        while (_hand.Count < 5)
        {
            DrawCard();
        }
    }

    public void Discard(PlayCard card)
    {
        if (_open.Contains(card)) _open.Remove(card);

        _discard.Add(card);
    }

    public void Shuffle()
    {
        while (0 < _discard.Count)
        {
            int random = Random.Range(0, _discard.Count);
            PlayCard card = _discard[random];
            _discard.RemoveAt(random);
            _deck.Add(card);
        }

        Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
    }

    private void BindEndTurnEvent()
    {
        TurnManager.Instance.OnEndMonsterActionState += EndMonsterActionState;
    }

    private void UnbindEndTurnEvent()
    {
        if (TurnManager.Instance == null) return;
        TurnManager.Instance.OnEndMonsterActionState -= EndMonsterActionState;
    }
}
