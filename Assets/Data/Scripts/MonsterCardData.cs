using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterCardData", menuName = "Scriptable Objects/MonsterCardData")]
public class MonsterCardData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private List<ActionType> _actionList1;
    [SerializeField] private List<ActionType> _actionList2;

    public int ID => _id;
    public List<ActionType> ActionList1 => _actionList1;
    public List<ActionType> ActionList2 => _actionList2;
}
