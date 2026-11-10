using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _drawAction;
    [SerializeField] private InputActionReference _useAction;
    [SerializeField] private InputActionReference _finishAction;
    [SerializeField] private DungeonMap _dungeonMap;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private Monster _monster;
    [SerializeField] private int _currentRemainMonster;
    [SerializeField] private DeckManager _deckManager;

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputAction();
    private void Start() => Init();
    private void OnDisable() => UnbindInputAction();

    public void Action(ActionType action)
    {
        if (action == ActionType.Draw)
        {
            _deckManager.DrawCard();
        }
        else if (action == ActionType.Move)
        {
            _monster.Move();
        }
        else if (action == ActionType.Activate)
        {
            _monster.Activate();
        }
        else if (action == ActionType.SRAttack)
        {
            _monster.SRAttack();
        }
        else if (action == ActionType.LRAttack)
        {
            _monster.LRAttack();
        }
        else if (action == ActionType.Defense)
        {
            _monster.Defense();
        }
        else if (ActionType.SummonInSameRoom <= action && action <= ActionType.SummonInJail)
        {
            SummonIn(action);
        }
        else if (ActionType.CreateCoin <= action && action <= ActionType.CreateTrap)
        {
            _monster.Create(action);
        }
    }

    public void InitSummon()
    {
        RoomType initSummonRoom = _clanData.CanSummonRoom;

        switch (initSummonRoom)
        {
            case RoomType.Sword:
                Action(ActionType.SummonInSword);
                return;
            case RoomType.Eye:
                Action(ActionType.SummonInEye);
                return;
            case RoomType.Hat:
                Action(ActionType.SummonInHat);
                return;
            case RoomType.Skull:
                Action(ActionType.SummonInSkull);
                return;
            case RoomType.Treasure:
                Action(ActionType.SummonInTreasure);
                return;
            default:
                Debug.Log($"초기 소환 방 타입 잘못됨");
                return;
        }
    }

    private void SummonIn(ActionType action)
    {
        switch (action)
        {
            case ActionType.SummonInSword:
                Summon(RoomType.Sword);
                return;
            case ActionType.SummonInEye:
                Summon(RoomType.Eye);
                return;
            case ActionType.SummonInHat:
                Summon(RoomType.Hat);
                return;
            case ActionType.SummonInSkull:
                Summon(RoomType.Skull);
                return;
            case ActionType.SummonInTreasure:
                Summon(RoomType.Treasure);
                return;
            default:
                Debug.Log($"소환 방 타입 잘못됨");
                return;
        }
    }

    private void Summon(RoomType room)
    {
        for(int i = 0; i < _dungeonMap.Rooms.Count; i++)
        {
            if (_dungeonMap.Rooms[i].RoomType == room)
            {

            }
        }
    }

    private void CacheComponents()
    {
        _deckManager = GetComponent<DeckManager>();
    }

    private void Init()
    {
        _currentRemainMonster = _clanData.MaxSummon;

        for (int i = 0; i < _clanData.MonsterCardList.Count; i++)
        {
            MonsterCardData data = _clanData.MonsterCardList[i];
            int times = _clanData.MonsterCardQuantity[i];

            for (int j = 0; j < times; j++)
            {
                MonsterPlayCard card = new MonsterPlayCard(CardState.Discard, data);
                _deckManager.Discard(card);
            }
        }

        MonsterData monsterData = _clanData.ClanMonster;
        _monster.SetData(monsterData);

        _deckManager.FinishTurn();
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
        _deckManager.DrawCard();
    }

    private void OnUseCard(InputAction.CallbackContext context)
    {
        _deckManager.UseCard(0);
    }

    private void OnFinishTurn(InputAction.CallbackContext context)
    {
        _deckManager.FinishTurn();
    }
}
