using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingCard", menuName = "Scriptable Objects/CraftingCard")]
public class CraftingCard : ScriptableObject
{
    [SerializeField] private CardType _cardType;
    [SerializeField] private List<ActionType> _actionList;

    public CardType CardType => _cardType;
    public List<ActionType> ActionList => _actionList;
}
