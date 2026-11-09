using UnityEngine;

[CreateAssetMenu(fileName = "TreasureData", menuName = "Scriptable Objects/TreasureData")]
public class TreasureData : ScriptableObject
{
    [SerializeField] private int _value;

    public int Value => _value;
}
