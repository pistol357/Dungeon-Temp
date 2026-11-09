using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    [SerializeField] private HeroType _heroType;
    [SerializeField] private int _maxHp;
    [SerializeField] private string _ability;

    public HeroType HeroType => _heroType;
    public int MaxHp => _maxHp;
    public string Ability => _ability;
}
