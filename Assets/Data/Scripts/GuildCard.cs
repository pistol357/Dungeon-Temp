using UnityEngine;

[CreateAssetMenu(fileName = "GuildCard", menuName = "Scriptable Objects/GuildCard")]
public class GuildCard : ScriptableObject
{
    [SerializeField] private HeroType _summonHero;
    [SerializeField] private string _summonRoom;
    [SerializeField] private int _summonQuantity;

    public HeroType SummonHero => _summonHero;
    public string SummonRoom => _summonRoom;
    public int SummonQuantity => _summonQuantity;
}
