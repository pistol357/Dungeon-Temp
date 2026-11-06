using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Scriptable Objects/Hero")]
public class Hero : ScriptableObject
{
    [SerializeField] private HeroType _heroType;
    [SerializeField] private int _maxHp;
    [SerializeField] private string _ability;

    public HeroType HeroType => _heroType;
    public int MaxHp => _maxHp;
    public string Ability => _ability;
}
