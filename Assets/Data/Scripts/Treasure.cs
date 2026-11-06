using UnityEngine;

[CreateAssetMenu(fileName = "Treasure", menuName = "Scriptable Objects/Treasure")]
public class Treasure : ScriptableObject
{
    [SerializeField] private int _value;

    public int Value => _value;
}
