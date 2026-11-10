using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildData", menuName = "Scriptable Objects/GuildData")]
public class GuildData : ScriptableObject
{
    [SerializeField] private int _maxSummon;
    [SerializeField] private HeroData _warrior;
    [SerializeField] private HeroData _archer;
    [SerializeField] private HeroData _rogue;
    [SerializeField] private List<GuildCardData> _guildCardList;

    public int MaxSummon => _maxSummon;
    public HeroData Warrior => _warrior;
    public HeroData Archer => _archer;
    public HeroData Rogue => _rogue;
    public List<GuildCardData> GuildCardList => _guildCardList;
}
