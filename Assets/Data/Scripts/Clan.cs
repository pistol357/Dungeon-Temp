using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clan", menuName = "Scriptable Objects/Clan")]
public class Clan : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Monster _clanMonster;
    [SerializeField] private string _ability;
    [SerializeField] private int _maxSummon;
    [SerializeField] private int _initialSummon;
    [SerializeField] private string _canSummonRoom;
    [SerializeField] private List<MonsterCard> _monsterCardList;
    [SerializeField] private List<int> _monsterCardQuantity;

    public string Name => _name;
    public Monster ClanMonster => _clanMonster;
    public string Ability => _ability;
    public int MaxSummon => _maxSummon;
    public int InitialSummon => _initialSummon;
    public string CanSummonRoom => _canSummonRoom;
    public List<MonsterCard> MonsterCardList => _monsterCardList;
    public List<int> MonsterCardQuantity => _monsterCardQuantity;
}
