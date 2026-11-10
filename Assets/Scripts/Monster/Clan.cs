using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _drawAction;
    [SerializeField] private InputActionReference _useAction;
    [SerializeField] private InputActionReference _finishAction;
    [SerializeField] private Transform _handUI;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private int _currentRemainMonster;
    [SerializeField] private List<PlayCard> _deck;
    [SerializeField] private List<PlayCard> _hand;
    [SerializeField] private List<PlayCard> _open;
    [SerializeField] private List<PlayCard> _discard;
    [SerializeField] private PlayCardObject _playCardPrefab;

    private void OnEnable() => BindInputAction();
    private void Start() => Init();
    private void OnDisable() => UnbindInputAction();

    private void DrawCard()
    {
        if(_deck.Count > 0)
        {
            PlayCard card = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(card);

            card.SetCardState(CardState.Hand);
            Debug.Log($"{card} 뽑음");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }
        else if(_deck.Count == 0)
        {
            if(_discard.Count == 0)
            {
                Debug.Log($"뽑을 카드 부족");
                return;
            }

            Shuffle();
            DrawCard();
        }
    }

    private void UseCard(PlayCard card)
    {
        if(_hand.Contains(card) && card.CardState == CardState.Hand)
        {
            card.SetCardState(CardState.Open);
            _hand.Remove(card);
            _open.Add(card);
            Debug.Log($"{card} 사용됨");
            Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
        }
        else
        {
            Debug.Log($"{card} 사용 불가");
        }
    }

    private void FinishTurn()
    {
        foreach(PlayCard card in _open)
        {
            card.SetCardState(CardState.Discard);
            _discard.Add(card);
        }

        _open.Clear();

        while(_hand.Count < 5)
        {
            DrawCard();
        }
    }

    private void Discard(PlayCard card)
    {
        if(_hand.Contains(card)) _hand.Remove(card);

        card.SetCardState(CardState.Discard);
        _discard.Add(card);
    }

    private void Shuffle()
    {
        while(0 < _discard.Count)
        {
            int random = Random.Range(0, _discard.Count);
            PlayCard card = _discard[random];
            _discard.RemoveAt(random);
            _deck.Add(card);
            card.SetCardState(CardState.Deck);
        }

        Debug.Log($"덱 : {_deck.Count} / 손 : {_hand.Count} / 공개 : {_open.Count} / 버림 : {_discard.Count}");
    }

    private void Init()
    {
        _currentRemainMonster = _clanData.MaxSummon;
        _deck = new();
        _hand = new();
        _open = new();
        _discard = new();

        for(int i = 0; i < _clanData.MonsterCardList.Count; i++)
        {
            MonsterCardData data = _clanData.MonsterCardList[i];
            int times = _clanData.MonsterCardQuantity[i];

            for(int j = 0; j < times; j++)
            {
                MonsterPlayCard card = new MonsterPlayCard(CardState.Discard, data);
                _discard.Add(card);
            }
        }

        FinishTurn();
    }

    private void BindInputAction()
    {
        _drawAction.action.Enable();
        _useAction.action.Enable();
        _finishAction.action.Enable();
        _drawAction.action.performed += OnDrawCard;
        _useAction.action.performed += OnUseCard;
        _finishAction.action.performed += OnFinishTurn;
    }

    private void UnbindInputAction()
    {
        _drawAction.action.Disable();
        _useAction.action.Disable();
        _finishAction.action.Disable();
        _drawAction.action.performed -= OnDrawCard;
        _useAction.action.performed -= OnUseCard;
        _finishAction.action.performed -= OnFinishTurn;
    }

    private void OnDrawCard(InputAction.CallbackContext context)
    {
        DrawCard();
    }

    private void OnUseCard(InputAction.CallbackContext context)
    {
        if(_hand.Count <= 0)
        {
            Debug.Log($"손 패 부족");
            return;
        }

        UseCard(_hand[0]);
    }

    private void OnFinishTurn(InputAction.CallbackContext context)
    {
        FinishTurn();
    }
}
