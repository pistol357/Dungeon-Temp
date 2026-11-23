using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour
{
    private const float UPDATE_TIME = 0.3f;

    [SerializeField] private GuildData _guildData;

    private Stack<Hero> _remainWarriors = new();
    private Stack<Hero> _remainArchers = new();
    private Stack<Hero> _remainRogues = new();
    private List<Hero> _activeHeroes = new();
    private List<GuildCard> _guildDeck = new();
    private List<GuildCard> _guildOpen = new();
    private GuildState _state;
    private Difficulty _difficulty = Difficulty.Normal;
    private int[,] _waveCard = { { 1, 2 }, { 2, 2 }, { 2, 3 } };
    private int _wave = 1;
    private int _drawCount;
    private HeroType _summonHero;
    private List<int> _summonRooms = new();
    private float _elapsedTime;

    public int Wave => _wave;
    public int RemainDeckCount => _guildDeck.Count;
    public int DeckCount => _guildDeck.Count + _guildOpen.Count;

    private void Start() => Init();
    private void Update() => Progress();

    public void ChangeGuildState(GuildState state)
    {
        _elapsedTime = 0;
        _state = state;
        CheckHeroState();

        Debug.Log($"길드 : {state}");
    }

    public void CheckHeroState()
    {
        for(int i = 0; i < _activeHeroes.Count;)
        {
            if (_activeHeroes[i].State == UnitState.None)
            {
                Return(_activeHeroes[i]);
                _activeHeroes.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    public void Return(Hero hero)
    {
        ActionManager.Instance.Return(hero);

        if (hero.HeroType == HeroType.Warrior)
        {
            _remainWarriors.Push(hero);
        }
        else if (hero.HeroType == HeroType.Archer)
        {
            _remainArchers.Push(hero);
        }
        else if (hero.HeroType == HeroType.Rogue)
        {
            _remainRogues.Push(hero);
        }
    }

    private void Progress()
    {
        _elapsedTime += Time.deltaTime;
        

        if (_elapsedTime < UPDATE_TIME || _state == GuildState.None)
        {
            return;
        }
        else if (_state == GuildState.Start)
        {
            StartTurn();
        }
        else if (_state == GuildState.Draw)
        {
            Draw();
        }
        else if (_state == GuildState.Summon)
        {
            Summon();
        }
        else if (_state == GuildState.SummonAction)
        {
            SummonAction();
        }
        else if (_state == GuildState.Enter)
        {
            Enter();
        }
        else if (_state == GuildState.Attack)
        {
            CheckAttack();
        }
        else if (_state == GuildState.Loot)
        {
            CheckTreasure();
        }
        else if (_state == GuildState.Move)
        {
            Move();
        }
        else if (_state == GuildState.End)
        {
            EndHeroInvasionState();
        }
    }

    private void StartTurn()
    {
        if (_guildDeck.Count == 0)
        {
            _wave++;
            Shuffle();
        }

        _drawCount = _waveCard[(int)_difficulty, _wave - 1];

        ChangeGuildState(GuildState.Draw);
    }

    private void Draw()
    {
        if (_guildDeck.Count == 0)
        {
            _drawCount = 0;
            ChangeGuildState(GuildState.End);
            return;
        }

        GuildCard card = _guildDeck[0];
        _guildDeck.RemoveAt(0);
        _guildOpen.Add(card);
        _drawCount--;

        _summonHero = card.HeroType;
        _summonRooms = DungeonMap.Instance.GetRoomNumbersOfType(card.RoomType);

        Debug.Log($"길드 : {card} 뽑음");
        Debug.Log($"길드 : {_guildDeck.Count} / 공개 : {_guildOpen.Count}");

        ChangeGuildState(GuildState.Summon);
    }

    private void Shuffle()
    {
        while (0 < _guildOpen.Count)
        {
            int random = Random.Range(0, _guildOpen.Count);
            GuildCard card = _guildOpen[random];
            _guildOpen.RemoveAt(random);
            _guildDeck.Add(card);
        }

        Debug.Log($"길드덱 : {_guildDeck.Count} / 공개 : {_guildOpen.Count}");
    }

    private void Summon()
    {
        for(int i = 0; i < _summonRooms.Count; i++)
        {
            IActionable hero = null;
            if (_summonHero == HeroType.Warrior)
            {
                if (_remainWarriors.Count > 0) hero = _remainWarriors.Pop();
            }
            else if (_summonHero == HeroType.Archer)
            {
                if (_remainArchers.Count > 0) hero = _remainArchers.Pop();
            }
            else if (_summonHero == HeroType.Rogue)
            {
                if (_remainRogues.Count > 0) hero = _remainRogues.Pop();
            }

            if (hero == null)
            {
                Debug.Log($"소환 가능한 영웅 없음. 해당 영웅 모두 활성화");

                foreach(Hero h in _activeHeroes)
                {
                    if(_summonHero == h.HeroType && h.State == UnitState.Exhausted)
                    {
                        h.Cheer();
                    }
                }

                ChangeGuildState(GuildState.SummonAction);
                return;
            }

            int room = _summonRooms[i];
            ActionManager.Instance.Summon(room, hero);
            _activeHeroes.Add(hero as Hero);
            (hero as Hero).Cheer();
        }

        ChangeGuildState(GuildState.SummonAction);
    }

    private void SummonAction()
    {
        ChangeGuildState(GuildState.Enter);
    }

    private void Enter()
    {
        foreach(Hero hero in _activeHeroes)
        {
            if (hero.State == UnitState.Active)
            {
                int roomNumber = hero.CurrentRoomNumber;
                Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

                if (room.IsTrapExist())
                {
                    room.RemoveResource(ResourceType.Trap);
                    hero.Damage(1);
                    if (hero.State == UnitState.None)
                    {
                        continue;
                    }
                }

                List<Hero> list = room.Heroes;

                if (list.Count > 0)
                {
                    foreach (Hero h in list)
                    {
                        if (h.State == UnitState.Exhausted)
                        {
                            h.Cheer();
                        }
                    }
                }
            }
        }

        ChangeGuildState(GuildState.Attack);
    }

    private void CheckAttack()
    {
        foreach (Hero hero in _activeHeroes)
        {
            if (hero.State == UnitState.Active)
            {
                int roomNumber = hero.CurrentRoomNumber;
                Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

                if (room.Monsters.Count > 0)
                {
                    IActionable target = room.Monsters[0];
                    ActionManager.Instance.Attack(target);
                    hero.Exhaust();
                }
            }
        }

        ChangeGuildState(GuildState.Loot);
    }

    private void CheckTreasure()
    {
        foreach (Hero hero in _activeHeroes)
        {
            if (hero.State == UnitState.Active)
            {
                int roomNumber = hero.CurrentRoomNumber;
                Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

                if (!room.Treasure.IsOpened)
                {
                    List<Hero> list = room.Heroes;
                    int count = 0;

                    foreach(Hero h in list)
                    {
                        if (h.State == UnitState.Active)
                        {
                            h.Exhaust();
                            count++;
                        }

                        if (count >= room.Treasure.Value)
                        {
                            room.Treasure.Open();

                            if (DungeonMap.Instance.IsTreasureRoomPlundered())
                            {
                                TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.GameOverState);
                                return;
                            }

                            break;
                        }
                    }
                }
            }
        }

        ChangeGuildState(GuildState.Move);
    }

    private void Move()
    {
        foreach(Hero hero in _activeHeroes)
        {
            if (hero.State == UnitState.Active)
            {
                int nextRoomNumber = DungeonMap.Instance.GetNextRoomNumber(hero.CurrentRoomNumber);
                ActionManager.Instance.Move(hero, nextRoomNumber, ResourceType.None);
            }
        }

        foreach(Hero hero in _activeHeroes)
        {
            if(hero.State == UnitState.Active)
            {
                ChangeGuildState(GuildState.Enter);
                return;
            }
        }

        ChangeGuildState(GuildState.End);
    }

    private void EndHeroInvasionState()
    {
        if (_wave == 2 && _guildDeck.Count == 0 && !DungeonMap.Instance.IsTreasureRoomPlundered())
        {
            TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.GameClearState);
        }
        else if (_drawCount > 0)
        {
            ChangeGuildState(GuildState.Draw);
        }
        else
        {
            TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.EndTurnState);
        }
    }

    private void Init()
    {
        int max = _guildData.MaxSummon;

        for (int i = 0; i < _guildData.HeroDatas.Count; i++)
        {
            for (int j = 0; j < max; j++)
            {
                HeroData heroData = _guildData.HeroDatas[i];
                HeroType heroType = heroData.HeroType;
                if(heroType == HeroType.Warrior)
                {
                    _remainWarriors.Push(new Hero(heroData, this));
                }
                else if(heroType == HeroType.Archer)
                {
                    _remainArchers.Push(new Hero(heroData, this));
                }
                else if(heroType == HeroType.Rogue)
                {
                    _remainRogues.Push(new Hero(heroData, this));
                }
            }
        }

        for (int i = 0; i < _guildData.GuildCardList.Count; i++)
        {
            GuildCardData data = _guildData.GuildCardList[i];
            GuildCard card = new GuildCard(data);

            _guildOpen.Add(card);
        }

        Shuffle();
    }
}
