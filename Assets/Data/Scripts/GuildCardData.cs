using UnityEngine;

[CreateAssetMenu(fileName = "GuildCardData", menuName = "Scriptable Objects/GuildCardData")]
public class GuildCardData : ScriptableObject
{
    [SerializeField] private HeroType _summonHero;
    [SerializeField] private RoomType _summonRoom;
    [SerializeField] private int _summonQuantity;

    public HeroType SummonHero => _summonHero;
    public RoomType SummonRoom => _summonRoom;
    public int SummonQuantity => _summonQuantity;
}
