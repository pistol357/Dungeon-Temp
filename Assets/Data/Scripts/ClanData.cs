using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClanData", menuName = "Scriptable Objects/ClanData")]
public class ClanData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private MonsterData _clanMonster;
    [SerializeField] private string _ability;
    [SerializeField] private int _maxSummon;
    [SerializeField] private int _initialSummon;
    [SerializeField] private RoomType _canSummonRoom;
    [SerializeField] private List<MonsterCardData> _monsterCardList;
    [SerializeField] private List<int> _monsterCardQuantity;

    public string Name => _name;
    public MonsterData ClanMonster => _clanMonster;
    public string Ability => _ability;
    public int MaxSummon => _maxSummon;
    public int InitialSummon => _initialSummon;
    public RoomType CanSummonRoom => _canSummonRoom;
    public List<MonsterCardData> MonsterCardList => _monsterCardList;
    public List<int> MonsterCardQuantity => _monsterCardQuantity;
}
