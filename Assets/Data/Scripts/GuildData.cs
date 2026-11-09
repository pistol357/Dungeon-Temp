using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildData", menuName = "Scriptable Objects/GuildData")]
public class GuildData : ScriptableObject
{
    [SerializeField] private int _maxSummon;
    [SerializeField] private HeroData _warriorList;
    [SerializeField] private HeroData _archerList;
    [SerializeField] private HeroData _rogueList;
    [SerializeField] private List<GuildCardData> _guildCardList;
}
