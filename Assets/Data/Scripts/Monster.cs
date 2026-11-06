using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class Monster : ScriptableObject
{
    [SerializeField] private int _maxHp;

    public int MaxHp => _maxHp;
}
