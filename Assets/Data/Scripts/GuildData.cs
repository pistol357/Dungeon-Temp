using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildData", menuName = "Scriptable Objects/GuildData")]
public class GuildData : ScriptableObject
{
    [SerializeField] private int _maxSummon;
    [SerializeField] private List<HeroData> _heroDatas;
    [SerializeField] private List<GuildCardData> _guildCardList;

    public int MaxSummon => _maxSummon;
    public List<HeroData> HeroDatas => _heroDatas;
    public List<GuildCardData> GuildCardList => _guildCardList;
}
