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

    private void DrawCard()
    {
        if(_deck.Count > 0)
        {
            int random = Random.Range(0, _deck.Count);
            PlayCard card = _deck[random];
            _deck.RemoveAt(random);
            _hand.Add(card);
            card.SetCardState(CardState.Hand);
            Instantiate(_playCardPrefab, _canvas.transform).SetCard(card);
            Debug.Log(card);
        }
        else if(_deck.Count == 0)
        {

        }
    }

    private void UseCard()
    {

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
                MonsterPlayCard card = new MonsterPlayCard(CardState.Deck, data);
                _deck.Add(card);
            }
        }
    }
}
