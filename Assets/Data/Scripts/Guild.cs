using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guild", menuName = "Scriptable Objects/Guild")]
public class Guild : ScriptableObject
{
    [SerializeField] private List<Hero> _warriorList;
    [SerializeField] private List<Hero> _archerList;
    [SerializeField] private List<Hero> _rogueList;
    [SerializeField] private List<GuildCard> _guildCardList;
}
