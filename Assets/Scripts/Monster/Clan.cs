using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _nextInput;
    [SerializeField] private InputActionReference _selectInput;
    [SerializeField] private InputActionReference _endInput;
    [SerializeField] private DungeonMap _dungeonMap;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private DeckManager _deckManager;

    private Stack<Monster> _remainMonsters = new();
    private List<Monster> _activeMonsters = new();
    private Monster _selectedMonster;
    private int _selectedTargetRoom;
    private bool _isWaitForInput;
    private WaitFor _state;
    private int _currentMonsterCardIndex;
    private int _currentActionListIndex;
    private int _currentActionIndex;
    private int _currentMonsterIndex;
    private int _currentRoomIndex;

    public int ActiveMonsterCount => _activeMonsters.Count;
    public DeckManager DeckManager => _deckManager;

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputAction();
    private void Start() => Init();
    private void OnDisable() => UnbindInputAction();

    public void Action(Monster monster, ActionType action)
    {
        if (action == ActionType.Draw)
        {
            _deckManager.DrawCard();
        }
        else if (action == ActionType.Move)
        {
            monster.Move();
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
            SummonByActionType(monster, action);
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

    private void SummonByActionType(Monster monster, ActionType action)
    {
        switch (action)
        {
            case ActionType.SummonInSameRoom:
                Summon(monster.CurrentRoomNumber);
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
        if(_remainMonsters.Count <= 0)
        {
            Debug.Log($"소환 불가(남은 몬스터 없음)");
            return;
        }

        Room room = _dungeonMap.GetRoomByNumber(roomNumber);
        Monster monster = _remainMonsters.Pop();

        room.AddMonster(monster);
        monster.SetRoom(room);
        _activeMonsters.Add(monster);
        Debug.Log($"{monster.Name}(이)가 {room.Name}에 매복함");
    }

    public void WaitForInput(bool state)
    {
        _isWaitForInput = state;
    }

    public void ChangeState(WaitFor state)
    {
        _state = state;
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
            _remainMonsters.Push(monster);
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

        _deckManager.Shuffle();
    }

    private void BindInputAction()
    {
        _nextInput.action.Enable();
        _selectInput.action.Enable();
        _endInput.action.Enable();
        _nextInput.action.performed += OnNext;
        _selectInput.action.performed += OnSelect;
        _endInput.action.performed += OnEnd;
    }

    private void UnbindInputAction()
    {
        _nextInput.action.Disable();
        _selectInput.action.Disable();
        _endInput.action.Disable();
        _nextInput.action.performed -= OnNext;
        _selectInput.action.performed -= OnSelect;
        _endInput.action.performed -= OnEnd;
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (!_isWaitForInput) return;

        if(_state == WaitFor.SelectCard)
        {
            NextCard();
        }
        else if (_state == WaitFor.SelectActionList)
        {
            NextActionList();
        }
        else if(_state == WaitFor.SelectAction)
        {
            NextAction();
        }
        else if(_state == WaitFor.SelectMonster)
        {
            NextMonster();
        }
        else if(_state == WaitFor.SelectTargetRoom)
        {
            NextRoom();
        }
        else if(_state == WaitFor.SelectTargetResource)
        {

        }
    }

    private void NextCard()
    {
        if (_deckManager.HandCount == 0) return;

        _currentMonsterCardIndex = (_currentMonsterCardIndex + 1) % _deckManager.HandCount;
    }

    private void NextActionList()
    {
        _currentActionListIndex = (_currentActionListIndex + 1) % _deckManager.SelectedCard.CardActionCount;
    }

    private void NextAction()
    {
        _currentActionIndex = (_currentActionIndex + 1) % _deckManager.SelectedActionLIstCount;
    }

    private void NextMonster()
    {
        _currentMonsterIndex = (_currentMonsterIndex + 1) % _activeMonsters.Count;
    }

    private void NextRoom()
    {
        _currentRoomIndex = (_currentRoomIndex + 1) % _dungeonMap.GetConnectedRoomList(_selectedMonster.CurrentRoomNumber).Count;
    }

    private void NextResource()
    {

    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (!_isWaitForInput) return;

        if (_state == WaitFor.SelectCard)
        {
            SelectCard();
        }
        else if (_state == WaitFor.SelectActionList)
        {
            SelectActionList();
        }
        else if (_state == WaitFor.SelectAction)
        {
            SelectAction();
        }
        else if (_state == WaitFor.SelectMonster)
        {
            SelectMonster();
        }
        else if (_state == WaitFor.SelectTargetRoom)
        {
            SelectTargetRoom();
        }
        else if (_state == WaitFor.SelectTargetResource)
        {

        }
    }

    private void SelectCard()
    {
        if (_deckManager.HandCount == 0) return;

        _deckManager.SelectCard(_currentMonsterCardIndex);
        _currentMonsterCardIndex = 0;
        ChangeState(WaitFor.SelectActionList);
    }

    private void SelectActionList()
    {
        _deckManager.SelectActionList(_currentActionListIndex);
        _currentActionListIndex = 0;
        ChangeState(WaitFor.SelectAction);
    }

    private void SelectAction()
    {
        _deckManager.SelectAction(_currentActionIndex);
        _currentActionIndex = 0;
        CheckAction();
        CheckActiveMonster();
    }

    private void CheckAction()
    {
        ActionType action = _deckManager.SelectedAction;

        if (action == ActionType.Draw
            || (ActionType.SummonInSword <= action && action <= ActionType.SummonInTreasure))
        {

        }
        else if (action == ActionType.Move
            || action == ActionType.Activate
            || action == ActionType.SRAttack
            || action == ActionType.LRAttack
            || action == ActionType.Defense
            || action == ActionType.SummonInSameRoom
            || (ActionType.CreateCoin <= action && action <= ActionType.CreateTrap))
        {
            CheckActiveMonster();
        }
    }

    private void CheckActiveMonster()
    {
        if (_activeMonsters.Count == 0)
        {
            Debug.Log($"선택 가능한 몬스터 없음");
            EndAction();
        }
        else if (_activeMonsters.Count == 1)
        {
            _selectedMonster = _activeMonsters[0];
            ChangeState(WaitFor.SelectTargetRoom);
        }
        else
        {
            ChangeState(WaitFor.SelectMonster);
        }
    }

    private void SelectMonster()
    {
        _selectedMonster = _activeMonsters[_currentMonsterIndex];
        _currentMonsterIndex = 0;
        ChangeState(WaitFor.SelectTargetRoom);
    }

    private void SelectTargetRoom()
    {
        _selectedTargetRoom = _dungeonMap.GetConnectedRoomList(_selectedMonster.CurrentRoomNumber)[_currentRoomIndex];
        _currentRoomIndex = 0;
        ChangeState(WaitFor.SelectTargetResource);
    }

    private void CheckResource()
    {
        
    }

    private void OnEnd(InputAction.CallbackContext context)
    {
        EndMonsterActionState();
    }

    private void EndAction()
    {
        _deckManager.EndAction();
        
    }

    private void EndActionList()
    {

    }

    private void EndMonsterActionState()
    {
        TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.HeroInvasionState);
    }
}
