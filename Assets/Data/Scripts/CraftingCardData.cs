using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingCardData", menuName = "Scriptable Objects/CraftingCardData")]
public class CraftingCardData : ScriptableObject
{
    [SerializeField] private CardType _cardType;
    [SerializeField] private List<ActionType> _actionList;

    public CardType CardType => _cardType;
    public List<ActionType> ActionList => _actionList;
}
