using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _nextInput;
    [SerializeField] private InputActionReference _selectInput;
    [SerializeField] private InputActionReference _unselectInput;
    [SerializeField] private InputActionReference _endInput;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private GameObject _monsterPrefab;

    private Stack<Monster> _remainMonsters = new();
    private List<Monster> _activeMonsters = new();
    private PlayCard _selectedCard;
    private List<ActionType> _selectedActionList;
    private ActionType _selectedAction;
    private Monster _selectedMonster;
    private int _selectedTargetRoom = -1;
    private Resource _selectedTargetResource;
    private WaitFor _state;
    private int _currentIndex;

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputAction();
    private void Start() => Init();
    private void OnDisable() => UnbindInputAction();

    public void ChangeState(WaitFor state)
    {
        _currentIndex = 0;
        _state = state;
        Debug.Log($"{_state}");
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
            Monster monster = new Monster(monsterData);
            _remainMonsters.Push(monster);
        }

        for (int i = 0; i < _clanData.MonsterCardList.Count; i++)
        {
            MonsterCardData data = _clanData.MonsterCardList[i];
            int times = _clanData.MonsterCardQuantity[i];

            for (int j = 0; j < times; j++)
            {
                MonsterPlayCard card = new MonsterPlayCard(data);
                _deckManager.Discard(card);
            }
        }

        _deckManager.Shuffle();
    }

    private void BindInputAction()
    {
        _nextInput.action.Enable();
        _selectInput.action.Enable();
        _unselectInput.action.Enable();
        _endInput.action.Enable();
        _nextInput.action.performed += OnNext;
        _selectInput.action.performed += OnSelect;
        _unselectInput.action.performed += OnUnselect;
        _endInput.action.performed += OnEnd;
    }

    private void UnbindInputAction()
    {
        _nextInput.action.Disable();
        _selectInput.action.Disable();
        _unselectInput.action.Disable();
        _endInput.action.Disable();
        _nextInput.action.performed -= OnNext;
        _selectInput.action.performed -= OnSelect;
        _unselectInput.action.performed -= OnUnselect;
        _endInput.action.performed -= OnEnd;
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (_state == WaitFor.None)
        {
            return;
        }
        else if(_state == WaitFor.SelectCard)
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
        else if(_state == WaitFor.SelectMoveRoom)
        {
            NextRoom();
        }
        else if (_state == WaitFor.SelectSummonRoom)
        {
            NextSummonRoom();
        }
        else if(_state == WaitFor.SelectMoveResource)
        {
            NextResource();
        }
    }

    private void NextCard()
    {
        if (_deckManager.HandCount == 0) return;

        _currentIndex = (_currentIndex + 1) % _deckManager.HandCount;
        _deckManager.GetCard(_currentIndex).PrintCardInfo();
    }

    private void NextActionList()
    {
        _currentIndex = (_currentIndex + 1) % _selectedCard.CardActionCount;
        _selectedCard.PrintActionListInfo(_currentIndex);
    }

    private void NextAction()
    {
        _currentIndex = (_currentIndex + 1) % _selectedActionList.Count;
        Debug.Log($"현재 행동 : {_selectedActionList[_currentIndex]} ");
    }

    private void NextMonster()
    {
        if (_activeMonsters.Count == 0) return;

        _currentIndex = (_currentIndex + 1) % _activeMonsters.Count;
        Debug.Log($"현재 유닛 : {_activeMonsters[_currentIndex]} ");
    }

    private void NextRoom()
    {
        _currentIndex = (_currentIndex + 1) % DungeonMap.Instance.GetConnectedRoomNumberList(_selectedMonster.CurrentRoomNumber).Count;
        int room = DungeonMap.Instance.GetConnectedRoomNumberList(_selectedMonster.CurrentRoomNumber)[_currentIndex];
        Debug.Log($"이동 방 : {DungeonMap.Instance.GetRoomByNumber(room).Name} ");
    }

    private void NextSummonRoom()
    {
        _currentIndex = (_currentIndex + 1) % DungeonMap.Instance.GetRoomNumbersOfType(ActionManager.Instance.ActionTypeToRoomType(_selectedAction, _selectedMonster)).Count;
        int room = DungeonMap.Instance.GetRoomNumbersOfType(ActionManager.Instance.ActionTypeToRoomType(_selectedAction, _selectedMonster))[_currentIndex];
        Debug.Log($"소환 방 : {DungeonMap.Instance.GetRoomByNumber(room).Name} ");
    }

    private void NextResource()
    {
        int resourceCount = DungeonMap.Instance.GetRoomResources(_selectedMonster.CurrentRoomNumber).Count;
        if (resourceCount == 0) return;

        _currentIndex = (_currentIndex + 1) % resourceCount;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (_state == WaitFor.None)
        {
            return;
        }
        else if (_state == WaitFor.SelectCard)
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
        else if (_state == WaitFor.SelectMoveRoom)
        {
            SelectTargetRoom();
        }
        else if (_state == WaitFor.SelectSummonRoom)
        {
            SelectSummonRoom();
        }
        else if (_state == WaitFor.SelectMoveResource)
        {
            SelectTargetResource();
        }
    }

    private void SelectCard()
    {
        if (_deckManager.HandCount == 0) return;

        _selectedCard = _deckManager.SelectCard(_currentIndex);
        CheckCard();
    }

    private void CheckCard()
    {
        _currentIndex = 0;

        if (_selectedCard.GetActionListsCount == 1)
        {
            SelectActionList();
        }
        else if (1 < _selectedCard.GetActionListsCount)
        {
            ChangeState(WaitFor.SelectActionList);
        }
    }

    private void SelectActionList()
    {
        _selectedActionList = _selectedCard.GetActionList(_currentIndex).ToList();
        Debug.Log($"{_selectedCard.PrintActionListInfo(_currentIndex)} 선택됨");
        CheckActionList();
    }

    private void CheckActionList()
    {
        _currentIndex = 0;

        if (_selectedActionList.Count <= 0)
        {
            _selectedActionList = null;
            EndCard();
        }
        else if(_selectedActionList.Count == 1)
        {
            SelectAction();
        }
        else if(1 < _selectedActionList.Count)
        {
            ChangeState(WaitFor.SelectAction);
        }
    }

    private void SelectAction()
    {
        _selectedAction = _selectedActionList[_currentIndex];
        Debug.Log($"현재 행동 : {_selectedAction} 선택됨");
        CheckAction();
    }

    private void CheckAction()
    {
        _currentIndex = 0;

        ActionType action = _selectedAction;
        if (action == ActionType.Draw)
        {
            Action();
        }
        else if (ActionType.SummonInSword <= action && action <= ActionType.SummonInTreasure)
        {
            ChangeState(WaitFor.SelectSummonRoom);
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
        _currentIndex = 0;

        if (_activeMonsters.Count == 0)
        {
            Debug.Log($"선택 가능한 몬스터 없음");
            return;
        }
        else if (_activeMonsters.Count == 1)
        {
            SelectMonster();
        }
        else
        {
            ChangeState(WaitFor.SelectMonster);
        }
    }

    private void SelectMonster()
    {
        _selectedMonster = _activeMonsters[_currentIndex];
        Debug.Log($"현재 유닛 : {_selectedMonster} 선택됨");

        ActionType action = _selectedAction;
        if (action == ActionType.Move)
        {
            ChangeState(WaitFor.SelectMoveRoom);
        }
        else if(ActionType.CreateCoin <= action && action <= ActionType.CreateTrap)
        {
            Action();
        }
    }

    private void SelectTargetRoom()
    {
        _selectedTargetRoom = DungeonMap.Instance.GetConnectedRoomNumberList(_selectedMonster.CurrentRoomNumber)[_currentIndex];
        Debug.Log($"이동 방 : {DungeonMap.Instance.GetRoomByNumber(_selectedTargetRoom).Name} 선택됨");
        CheckRoom();
    }

    private void SelectSummonRoom()
    {
        _selectedTargetRoom = DungeonMap.Instance.GetRoomNumbersOfType(ActionManager.Instance.ActionTypeToRoomType(_selectedAction, _selectedMonster))[_currentIndex];
        Debug.Log($"소환 방 : {DungeonMap.Instance.GetRoomByNumber(_selectedTargetRoom).Name} 선택됨");
        Action();
    }

    private void CheckRoom()
    {
        _currentIndex = 0;

        List<Resource> resources = DungeonMap.Instance.GetRoomResources(_selectedMonster.CurrentRoomNumber);
        if (resources.Count == 0)
        {
            Action();
        }
        else if(0 < resources.Count)
        {
            ChangeState(WaitFor.SelectMoveResource);
        }
    }

    private void SelectTargetResource()
    {
        _selectedTargetResource = DungeonMap.Instance.GetRoomResources(_selectedMonster.CurrentRoomNumber)[_currentIndex];
        Action();
    }

    private void OnUnselect(InputAction.CallbackContext context)
    {
        if (_state == WaitFor.None)
        {
            return;
        }
        else if (_state == WaitFor.SelectCard)
        {
            return;
        }
        else if (_state == WaitFor.SelectActionList)
        {
            UnselectCard();
        }
        else if (_state == WaitFor.SelectAction)
        {
            UnselectActionList();
        }
        else if (_state == WaitFor.SelectSummonRoom)
        {
            UnselectAction();
        }
        else if (_state == WaitFor.SelectMonster)
        {
            UnselectAction();
        }
        else if (_state == WaitFor.SelectMoveRoom)
        {
            UnselectMonster();
        }
        else if (_state == WaitFor.SelectMoveResource)
        {
            UnselectTargetRoom();
        }
    }

    private void UnselectCard()
    {
        _deckManager.UnselectCard(_selectedCard);
        _selectedCard = null;
        ChangeState(WaitFor.SelectCard);
    }

    private void UnselectActionList()
    {
        Debug.Log($"{ActionListInfo()} 선택 취소");
        _selectedActionList = null;
        ChangeState(WaitFor.SelectActionList);
    }

    private string ActionListInfo()
    {
        string info = "";
        foreach (ActionType action in _selectedActionList)
        {
            info += $"{action} ";
        }

        return info;
    }

    private void UnselectAction()
    {
        Debug.Log($"{_selectedAction} 선택 취소");
        _selectedAction = ActionType.None;
        ChangeState(WaitFor.SelectAction);
    }

    private void UnselectMonster()
    {
        Debug.Log($"{_selectedMonster} 선택 취소");
        _selectedMonster = null;
        ChangeState(WaitFor.SelectMonster);
    }

    private void UnselectTargetRoom()
    {
        Debug.Log($"{_selectedTargetRoom} 선택 취소");
        _selectedTargetRoom = -1;
        ChangeState(WaitFor.SelectMoveRoom);
    }

    private void OnEnd(InputAction.CallbackContext context)
    {
        if (_state == WaitFor.None)
        {
            return;
        }
        else if (_state == WaitFor.SelectCard)
        {
            EndMonsterActionState();
        }
        else if (_state == WaitFor.SelectActionList)
        {
            _deckManager.Discard(_selectedCard);
            EndCard();
        }
    }

    private void EndMonsterActionState()
    {
        TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.HeroInvasionState);
    }

    private void EndAction()
    {
        _selectedActionList?.Remove(_selectedAction);
        _selectedAction = ActionType.None;
        _selectedMonster = null;
        _selectedTargetRoom = -1;
        _selectedTargetResource = null;
        CheckActionList();
    }

    private void EndCard()
    {
        _selectedCard = null;

        if (TurnManager.Instance.CurrentRound == 0)
        {
            EndMonsterActionState();
        }
        else
        {
            ChangeState(WaitFor.SelectCard);
        }
    }

    public void Action()
    {
        ActionType action = _selectedAction;
        if (action == ActionType.Draw)
        {
            _deckManager.DrawCard();
        }
        else if (action == ActionType.Move)
        {
            ActionManager.Instance.Move(_selectedMonster, _selectedTargetRoom, _selectedTargetResource);
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
            if (_remainMonsters.Count < 1)
            {
                Debug.Log($"소환 불가(남은 몬스터 없음)");
            }
            else
            {
                Monster monster = _remainMonsters.Pop();

                ActionManager.Instance.Summon(_selectedTargetRoom, monster);
                _activeMonsters.Add(monster);
            }
        }
        else if (ActionType.CreateCoin <= action && action <= ActionType.CreateTrap)
        {
            ActionManager.Instance.Create(_selectedMonster, action);
        }

        EndAction();
    }

    public void InitSummon()
    {
        RoomType initSummonRoom = _clanData.CanSummonRoom;

        _selectedAction = ActionManager.Instance.RoomTypeToActionType(initSummonRoom);
        if (_selectedActionList == null) _selectedActionList = new();
        _selectedActionList.Add(_selectedAction);

        ChangeState(WaitFor.SelectSummonRoom);
    }
}
