using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<PlayCard> _deck = new();
    [SerializeField] private List<PlayCard> _hand = new();
    [SerializeField] private List<PlayCard> _open = new();
    [SerializeField] private List<PlayCard> _discard = new();
    [SerializeField] private PlayCardObject _playCardPrefab;

    private PlayCard _selectedCard;
    private List<ActionType> _selectedActionList;
    private ActionType _selectedAction;

    public int HandCount => _hand.Count;
    public PlayCard SelectedCard => _selectedCard;
    public bool IsActionListSelected => _selectedActionList != null;
    public int SelectedActionLIstCount => _selectedActionList.Count;
    public ActionType SelectedAction => _selectedAction;

    private void OnEnable() => BindEndTurnEvent();
    private void OnDisable() => UnbindEndTurnEvent();

    public void DrawCard()
    {
        if (_deck.Count > 0)
        {
            PlayCard card = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(card);

            card.SetCardState(CardState.Hand);
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

    public void SelectCard(PlayCard card)
    {
        if (_hand.Count <= 0)
        {
            Debug.Log($"손 패 부족");
            return;
        }

        if (_hand.Contains(card) && card.CardState == CardState.Hand)
        {
            card.SetCardState(CardState.Open);
            _hand.Remove(card);
            _open.Add(card);
            _selectedCard = card;
            
            Debug.Log($"{card} 선택됨");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }
        else
        {
            Debug.Log($"{card} 사용 불가");
        }
    }

    public void SelectCard(int num)
    {
        if (_hand.Count <= 0)
        {
            Debug.Log($"손 패 부족");
            return;
        }

        if (num < 0 || _hand.Count <= num)
        {
            Debug.Log($"해당 카드 없음(인덱스 범위 벗어남)");
            return;
        }

        SelectCard(_hand[num]);
    }

    public void SelectActionList(int num)
    {
        if(_selectedCard.CardActionCount == 0)
        {
            Debug.Log($"카드 데이터 이상");
            return;
        }

        _selectedActionList = _selectedCard.GetActionList(num).ToList();
    }

    public void SelectAction(int num)
    {
        _selectedAction = _selectedActionList[num];
    }

    public void EndAction()
    {
        _selectedActionList.Remove(_selectedAction);
        _selectedAction = ActionType.None;
    }

    public void EndMonsterActionState()
    {
        foreach (PlayCard card in _open)
        {
            card.SetCardState(CardState.Discard);
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
        if (_hand.Contains(card)) _hand.Remove(card);

        card.SetCardState(CardState.Discard);
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
            card.SetCardState(CardState.Deck);
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
