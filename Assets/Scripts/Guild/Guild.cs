using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour
{
    [SerializeField] private GuildData _guildData;

    private Stack<Hero> _remainWarriors = new();
    private Stack<Hero> _remainArcher = new();
    private Stack<Hero> _remainRogue = new();
    private List<Hero> _activeHeroes = new();
    private List<Hero> _exhaustedHeroes = new();
    private List<GuildCard> _guildDeck = new();
    private List<GuildCard> _guildOpen = new();
    private GuildState _state;
    private int _wave = 1;
    private Vector2Int _difficulty = new Vector2Int(1, 2);
    private HeroType _summonHero;
    private List<int> _summonRooms = new();

    private void Start() => Init();
    private void Update() => Progress();

    public void ChangeGuildState(GuildState state)
    {
        _state = state;

        Debug.Log($"길드 : {state}");
    }

    private void Progress()
    {
        if (_state == GuildState.None)
        {
            return;
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
        else if (_state == GuildState.End)
        {
            EndHeroInvasionState();
        }
    }

    private void Draw()
    {
        if(_guildDeck.Count == 0)
        {
            _wave++;
            Shuffle();
        }

        GuildCard card = _guildDeck[0];
        _guildDeck.RemoveAt(0);
        _guildOpen.Add(card);
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
                hero = _remainWarriors?.Pop();
            }
            else if (_summonHero == HeroType.Archer)
            {
                hero = _remainArcher?.Pop();
            }
            else if (_summonHero == HeroType.Rogue)
            {
                hero = _remainRogue?.Pop();
            }

            if (hero == null)
            {
                Debug.Log($"소환 가능한 영웅 없음");
                return;
            }

            int room = _summonRooms[i];
            ActionManager.Instance.Summon(room, hero);
            _activeHeroes.Add(hero as Hero);
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
            int roomNumber = hero.CurrentRoomNumber;
            Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

            if (room.IsTrapExist())
            {
                ActionManager.Instance.Return(roomNumber, hero);
                Return(hero);
            }
        }

        ChangeGuildState(GuildState.End);
    }

    private void Return(Hero hero)
    {
        if (_activeHeroes.Contains(hero))
        {
            _activeHeroes.Remove(hero);
        }
        else if (_exhaustedHeroes.Contains(hero))
        {
            _exhaustedHeroes.Remove(hero);
        }

        // todo 종류에 따라 remain으로 넣어주기
    }

    private void EndHeroInvasionState()
    {
        TurnManager.Instance.StateMachine.ChangeState(TurnManager.Instance.EndTurnState);
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
                    _remainWarriors.Push(new Hero(heroData));
                }
                else if(heroType == HeroType.Archer)
                {
                    _remainArcher.Push(new Hero(heroData));
                }
                else if(heroType == HeroType.Rogue)
                {
                    _remainRogue.Push(new Hero(heroData));
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
