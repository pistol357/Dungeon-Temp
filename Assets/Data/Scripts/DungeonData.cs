using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "Scriptable Objects/DungeonData")]
public class DungeonData : ScriptableObject
{
    [SerializeField] private List<PlayCardData> _craftCards;

    public List<PlayCardData> CraftCards => _craftCards;
}
