using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClanData", menuName = "Scriptable Objects/ClanData")]
public class ClanData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private MonsterData _clanMonster;
    [SerializeField] private ActionType _activateAction;
    [SerializeField] private int _maxSummon;
    [SerializeField] private int _initialSummon;
    [SerializeField] private RoomType _canSummonRoom;
    [SerializeField] private List<PlayCardData> _monsterCardList;
    [SerializeField] private List<int> _monsterCardQuantity;

    public string Name => _name;
    public MonsterData ClanMonster => _clanMonster;
    public ActionType ActivateAction => _activateAction;
    public int MaxSummon => _maxSummon;
    public int InitialSummon => _initialSummon;
    public RoomType CanSummonRoom => _canSummonRoom;
    public List<PlayCardData> MonsterCardList => _monsterCardList;
    public List<int> MonsterCardQuantity => _monsterCardQuantity;
}
