using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clan : MonoBehaviour
{
    [SerializeField] private InputActionReference _prevInput;
    [SerializeField] private InputActionReference _nextInput;
    [SerializeField] private InputActionReference _selectInput;
    [SerializeField] private InputActionReference _unselectInput;
    [SerializeField] private InputActionReference _endInput;
    [SerializeField] private ClanData _clanData;
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private GameObject _monsterPrefab;

    private Stack<Monster> _remainMonsters = new();
    private List<Monster> _activeMonsters = new();
    private List<PlayCard> _canSelectCards = new();
    private PlayCard _selectedCard;
    private List<List<ActionType>> _canSelectActionLists = new();
    private List<ActionType> _selectedActionList;
    private List<ActionType> _canSelectActions = new();
    private ActionType _selectedAction;
    private List<Monster> _canSelectMonsters = new();
    private Monster _selectedMonster;
    private List<Hero> _canSelectHeroes = new();
    private Hero _selectedTargetHero;
    private List<int> _canSelectRooms = new();
    private int _selectedTargetRoom = -1;
    private List<Resource> _canSelectResources = new();
    private Resource _selectedTargetResource;
    private WaitFor _state;
    private int _canSelectCount;
    private int _currentIndex;

    public DeckManager DeckManager => _deckManager;
    public int CurrentIndex => _currentIndex;

    private void Awake() => CacheComponents();
    private void OnEnable() => BindInputAction();
    private void Start() => Init();
    private void OnDisable() => UnbindInputAction();

    public void InitSummon()
    {
        RoomType initSummonRoom = _clanData.CanSummonRoom;

        _selectedAction = ActionManager.Instance.RoomTypeToActionType(initSummonRoom);
        if (_selectedActionList == null) _selectedActionList = new();
        _selectedActionList.Add(_selectedAction);

        ChangeState(WaitFor.SelectSummonRoom);
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
            // Activate();
        }
        else if (action == ActionType.SRAttack || action == ActionType.LRAttack)
        {
            ActionManager.Instance.Attack(_selectedTargetHero);
        }
        else if (action == ActionType.Defense)
        {
            // Defense();
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

    public void ChangeState(WaitFor state)
    {
        _currentIndex = 0;
        _state = state;

        Debug.Log($"{_state}");

        if (_state == WaitFor.SelectCard)
        {
            _canSelectCards = _deckManager.Hand.ToList();
            _canSelectCount = _canSelectCards.Count;
        }
        else if (_state == WaitFor.SelectActionList)
        {
            _canSelectActionLists.Clear();

            for(int i = 0; i < _selectedCard.CardActionCount; i++)
            {
                _canSelectActionLists.Add(_selectedCard.GetActionList(i).ToList());
            }
            _canSelectCount = _canSelectActionLists.Count;

            if (_canSelectCount == 1)
            {
                SelectActionList();
                return;
            }
        }
        else if (_state == WaitFor.SelectAction)
        {
            _canSelectActions = _selectedActionList.ToList();
            _canSelectCount = _canSelectActions.Count;

            if (_canSelectCount == 0)
            {
                if (TurnManager.Instance.CurrentRound == 0)
                {
                    EndMonsterActionState();
                }
                else
                {
                    EndCard();
                }
            }
        }
        else if (_state == WaitFor.SelectMonster)
        {
            _canSelectMonsters = _activeMonsters.ToList();
            _canSelectCount = _canSelectMonsters.Count;

            if (_canSelectCount == 0)
            {
                Debug.Log($"선택 가능한 몬스터 없음");
                UnselectAction();
                return;
            }
        }
        else if (_state == WaitFor.SelectTargetHero)
        {
            if (_selectedAction == ActionType.SRAttack)
            {
                _canSelectHeroes = DungeonMap.Instance.GetRoomByNumber(_selectedMonster.CurrentRoomNumber).Heroes.ToList();
            }
            else if (_selectedAction == ActionType.LRAttack)
            {
                _canSelectHeroes.Clear();

                List<int> connectedRoomNumbers = DungeonMap.Instance.GetConnectedRoomNumberList(_selectedMonster.CurrentRoomNumber);
                for(int i = 0; i < connectedRoomNumbers.Count; i++)
                {
                    List<Hero> heroes = DungeonMap.Instance.GetRoomByNumber(connectedRoomNumbers[i]).Heroes.ToList();
                    foreach(Hero hero in heroes)
                    {
                        _canSelectHeroes.Add(hero);
                    }
                }
            }
            _canSelectCount = _canSelectHeroes.Count;

            if (_canSelectCount == 0)
            {
                Debug.Log($"선택 가능한 영웅 없음");
                UnselectAction();
                return;
            }
        }
        else if (_state == WaitFor.SelectSummonRoom)
        {
            _canSelectRooms = DungeonMap.Instance.GetRoomNumbersOfType(ActionManager.Instance.ActionTypeToRoomType(_selectedAction, _selectedMonster)).ToList();
            _canSelectCount = _canSelectRooms.Count;
        }
        else if (_state == WaitFor.SelectMoveRoom)
        {
            _canSelectRooms = DungeonMap.Instance.GetConnectedRoomNumberList(_selectedMonster.CurrentRoomNumber).ToList();
            _canSelectCount = _canSelectRooms.Count;
        }
        else if (_state == WaitFor.SelectMoveResource)
        {
            _canSelectResources = DungeonMap.Instance.GetRoomResources(_selectedMonster.CurrentRoomNumber).ToList();
            _canSelectCount = _canSelectResources.Count;

            if (_canSelectCount == 0)
            {
                Action();
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (_canSelectCount == 0 || _state == WaitFor.None)
        {
            Debug.Log($"선택 불가능");
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
        else if (_state == WaitFor.SelectTargetHero)
        {
            SelectTargetHero();
        }
        else if (_state == WaitFor.SelectSummonRoom || _state == WaitFor.SelectMoveRoom)
        {
            SelectTargetRoom();
        }
        else if (_state == WaitFor.SelectMoveResource)
        {
            SelectTargetResource();
        }
    }

    private void SelectCard()
    {
        _selectedCard = _canSelectCards[_currentIndex];
        _deckManager.SelectCard(_currentIndex);
        ChangeState(WaitFor.SelectActionList);
    }

    private void SelectActionList()
    {
        _selectedActionList = _canSelectActionLists[_currentIndex];
        Debug.Log($"{_selectedCard.PrintActionListInfo(_currentIndex)} 선택됨");
        ChangeState(WaitFor.SelectAction);
    }

    private void SelectAction()
    {
        _selectedAction = _canSelectActions[_currentIndex];
        Debug.Log($"현재 행동 : {_selectedAction} 선택됨");
        CheckAction();
    }

    private void CheckAction()
    {
        ActionType action = _selectedAction;
        if (action == ActionType.Draw)
        {
            Action();
        }
        else if (action == ActionType.Move
            || action == ActionType.Activate
            || action == ActionType.SRAttack
            || action == ActionType.LRAttack
            || action == ActionType.SummonInSameRoom
            || (ActionType.CreateCoin <= action && action <= ActionType.CreateTrap))
        {
            ChangeState(WaitFor.SelectMonster);
        }
        else if (ActionType.SummonInSword <= action && action <= ActionType.SummonInTreasure)
        {
            ChangeState(WaitFor.SelectSummonRoom);
        }
    }

    private void SelectMonster()
    {
        _selectedMonster = _canSelectMonsters[_currentIndex];
        Debug.Log($"현재 유닛 : {_selectedMonster} 선택됨");

        ActionType action = _selectedAction;
        if (action == ActionType.SummonInSameRoom
            || ActionType.CreateCoin <= action && action <= ActionType.CreateTrap)
        {
            Action();
        }
        else if (action == ActionType.Move)
        {
            ChangeState(WaitFor.SelectMoveRoom);
        }
        else if (action == ActionType.SRAttack
            || action == ActionType.LRAttack)
        {
            ChangeState(WaitFor.SelectTargetHero);
        }
    }

    private void SelectTargetRoom()
    {
        _selectedTargetRoom = _canSelectRooms[_currentIndex];
        Debug.Log($"방 : {DungeonMap.Instance.GetRoomByNumber(_selectedTargetRoom).Name} 선택됨");

        ActionType action = _selectedAction;
        if (action == ActionType.Move)
        {
            ChangeState(WaitFor.SelectMoveResource);
        }
        else if (ActionType.SummonInSword <= action && action <= ActionType.SummonInTreasure)
        {
            Action();
        }
    }

    private void SelectTargetHero()
    {
        _selectedTargetHero = _canSelectHeroes[_currentIndex];
        Debug.Log($"공격 대상 : {_selectedTargetHero} 선택됨");
        Action();
    }

    private void SelectTargetResource()
    {
        _selectedTargetResource = _canSelectResources[_currentIndex];
        Debug.Log($"방 : {_selectedTargetResource} 선택됨");
        Action();
    }

    private void OnUnselect(InputAction.CallbackContext context)
    {
        if (_state == WaitFor.None || _state == WaitFor.SelectCard)
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
        else if (_state == WaitFor.SelectMoveRoom || _state == WaitFor.SelectTargetHero)
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
        Debug.Log($"{_selectedActionList} 선택 취소");
        _selectedActionList = null;
        ChangeState(WaitFor.SelectActionList);
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
            EndCard();
        }
    }

    private void EndMonsterActionState()
    {
        TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.HeroInvasionState);
    }

    private void EndCard()
    {
        _selectedCard = null;
        _canSelectCards.Clear();

        if (TurnManager.Instance.CurrentRound == 0)
        {
            EndMonsterActionState();
        }
        else
        {
            ChangeState(WaitFor.SelectCard);
        }
    }

    private void EndAction()
    {
        _selectedActionList?.Remove(_selectedAction);
        _selectedAction = ActionType.None;
        _selectedMonster = null;
        _selectedTargetHero = null;
        _selectedTargetRoom = -1;
        _selectedTargetResource = null;
        TurnManager.Instance.Guild.CheckHeroState();
        ChangeState(WaitFor.SelectAction);
    }

    private void CacheComponents()
    {
        _deckManager = GetComponent<DeckManager>();
    }

    private void Init()
    {
        int max = _clanData.MaxSummon;

        for (int i = 0; i < max; i++)
        {
            MonsterData monsterData = _clanData.ClanMonster;
            Monster monster = new Monster(monsterData);
            _remainMonsters.Push(monster);
        }

        for (int i = 0; i < _clanData.MonsterCardList.Count; i++)
        {
            PlayCardData data = _clanData.MonsterCardList[i];
            int times = _clanData.MonsterCardQuantity[i];

            for (int j = 0; j < times; j++)
            {
                PlayCard card = new PlayCard(data);
                _deckManager.Discard(card);
            }
        }

        _deckManager.Shuffle();
    }

    private void BindInputAction()
    {
        _prevInput.action.Enable();
        _nextInput.action.Enable();
        _selectInput.action.Enable();
        _unselectInput.action.Enable();
        _endInput.action.Enable();
        _prevInput.action.performed += OnPrev;
        _nextInput.action.performed += OnNext;
        _selectInput.action.performed += OnSelect;
        _unselectInput.action.performed += OnUnselect;
        _endInput.action.performed += OnEnd;
    }

    private void UnbindInputAction()
    {
        _prevInput.action.Disable();
        _nextInput.action.Disable();
        _selectInput.action.Disable();
        _unselectInput.action.Disable();
        _endInput.action.Disable();
        _prevInput.action.performed -= OnPrev;
        _nextInput.action.performed -= OnNext;
        _selectInput.action.performed -= OnSelect;
        _unselectInput.action.performed -= OnUnselect;
        _endInput.action.performed -= OnEnd;
    }

    private void OnPrev(InputAction.CallbackContext context)
    {
        if (_canSelectCount == 0)
        {
            Debug.Log($"선택 불가능");
            return;
        }

        _currentIndex = (_currentIndex + _canSelectCount - 1) % _canSelectCount;

        PrintCurrentSelect();
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (_canSelectCount == 0)
        {
            Debug.Log($"선택 불가능");
            return;
        }

        _currentIndex = (_currentIndex + 1) % _canSelectCount;

        PrintCurrentSelect();
    }

    private void PrintCurrentSelect()
    {
        if (_state == WaitFor.SelectCard)
        {
            _canSelectCards[_currentIndex].PrintCardInfo();
        }
        else if (_state == WaitFor.SelectActionList)
        {
            _selectedCard.PrintActionListInfo(_currentIndex);
        }
        else if (_state == WaitFor.SelectAction)
        {
            Debug.Log($"현재 행동 : {_canSelectActions[_currentIndex]} ");
        }
        else if (_state == WaitFor.SelectMonster)
        {
            Debug.Log($"현재 유닛 : {_canSelectMonsters[_currentIndex]} ");
        }
        else if (_state == WaitFor.SelectTargetHero)
        {
            Debug.Log($"공격 대상 : {_canSelectHeroes[_currentIndex]} ");
        }
        else if (_state == WaitFor.SelectSummonRoom || _state == WaitFor.SelectMoveRoom)
        {
            Debug.Log($"방 : {DungeonMap.Instance.GetRoomByNumber(_canSelectRooms[_currentIndex]).Name} ");
        }
    }
}
