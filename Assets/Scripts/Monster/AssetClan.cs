using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssetClan : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputAction;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Clan _clanData;
    [SerializeField] private int _currentRemainMonster;
    [SerializeField] private List<PlayCard> _deck;
    [SerializeField] private List<PlayCard> _hand;
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
            Instantiate(_playCardPrefab, _canvas.transform).SetCard(card);
            Debug.Log(card);
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
        }
    }

    private void FinishTurn()
    {
        foreach(PlayCard card in _hand)
        {
            if (card.CardState == CardState.Open) Discard(card);
        }

        while(_hand.Count < 5)
        {
            DrawCard();
        }
    }

    private void Discard(PlayCard card)
    {
        if(_hand.Contains(card)) _hand.Remove(card);
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
    }

    private void Init()
    {
        _currentRemainMonster = _clanData.MaxSummon;
        _deck = new();
        _hand = new();
        _discard = new();

        for(int i = 0; i < _clanData.MonsterCardList.Count; i++)
        {
            MonsterCard data = _clanData.MonsterCardList[i];
            int times = _clanData.MonsterCardQuantity[i];

            for(int j = 0; j < times; j++)
            {
                MonsterPlayCard card = new MonsterPlayCard(CardState.Fin, data);
                _discard.Add(card);
            }
        }

        Shuffle();
    }

    private void BindInputAction()
    {
        _inputAction.action.Enable();
        _inputAction.action.performed += OnDrawCard;
    }

    private void UnbindInputAction()
    {
        _inputAction.action.Disable();
        _inputAction.action.performed -= OnDrawCard;
    }

    private void OnDrawCard(InputAction.CallbackContext context)
    {
        DrawCard();
    }
}
