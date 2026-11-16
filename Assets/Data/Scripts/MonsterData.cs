using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxHp;

    public Sprite Icon => _icon;
    public int MaxHp => _maxHp;
}
