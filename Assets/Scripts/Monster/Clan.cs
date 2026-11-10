using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _drawAction;
    [SerializeField] private InputActionReference _useAction;
    [SerializeField] private InputActionReference _finishAction;
    [SerializeField] private DungeonMap _dungeonMap;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private DeckManager _deckManager;

    private Stack<Monster> _monsters = new();

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
            // _monster.Move();
        }
        else if (action == ActionType.Activate)
        {
            // _monster.Activate();
        }
        else if (action == ActionType.SRAttack)
        {
            // _monster.SRAttack();
        }
        else if (action == ActionType.LRAttack)
        {
            // _monster.LRAttack();
        }
        else if (action == ActionType.Defense)
        {
            // _monster.Defense();
        }
        else if (ActionType.SummonInSameRoom <= action && action <= ActionType.SummonInJail)
        {
            SummonByActionType(action);
        }
        else if (ActionType.CreateCoin <= action && action <= ActionType.CreateTrap)
        {
            // _monster.Create(action);
        }
    }

    public void InitSummon()
    {
        RoomType initSummonRoom = _clanData.CanSummonRoom;

        SummonIn(initSummonRoom);
        TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.EndTurnState);
    }

    private void SummonByActionType(ActionType action)
    {
        switch (action)
        {
            case ActionType.SummonInSameRoom:
                // Summon(_monster);
                return;
            case ActionType.SummonInSword:
                SummonIn(RoomType.Sword);
                return;
            case ActionType.SummonInEye:
                SummonIn(RoomType.Eye);
                return;
            case ActionType.SummonInHat:
                SummonIn(RoomType.Hat);
                return;
            case ActionType.SummonInSkull:
                SummonIn(RoomType.Skull);
                return;
            case ActionType.SummonInTreasure:
                SummonIn(RoomType.Treasure);
                return;
            default:
                Debug.Log($"소환 방 타입 잘못됨");
                return;
        }
    }

    private void SummonIn(RoomType roomType)
    {
        List<Room> canSummonRooms = new();

        for(int i = 0; i < _dungeonMap.Rooms.Count; i++)
        {
            Room room = _dungeonMap.Rooms[i];
            if (room.RoomType == roomType)
            {
                canSummonRooms.Add(room);
            }
        }

        if(canSummonRooms.Count <= 0)
        {
            Debug.Log($"던전 방 그래프 이상");
        }
        else if(canSummonRooms.Count == 1)
        {
            Summon(canSummonRooms[0].RoomNumber);
        }
        else
        {
            Debug.Log($"소환 방 선택");
        }
    }

    private void Summon(int roomNumber)
    {
        if(_monsters.Count <= 0)
        {
            Debug.Log($"소환 불가(남은 몬스터 없음)");
            return;
        }

        Room room = _dungeonMap.GetRoomByNumber(roomNumber);
        Monster monster = _monsters.Pop();

        room.AddMonster(monster);
        monster.SetRoom(room);
        Debug.Log($"{monster.Name}(이)가 {room.Name}에 매복함");
    }

    private void CacheComponents()
    {
        _deckManager = GetComponent<DeckManager>();
    }

    private void Init()
    {
        int max = _clanData.MaxSummon;

        for(int i = 0; i < max; i++)
        {
            MonsterData monsterData = _clanData.ClanMonster;
            Monster monster = new Monster();
            monster.SetData(monsterData);
            _monsters.Push(monster);
        }

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
