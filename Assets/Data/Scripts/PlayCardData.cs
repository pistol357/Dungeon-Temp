using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayCardData", menuName = "Scriptable Objects/PlayCardData")]
public class PlayCardData : ScriptableObject
{
    [SerializeField] private CardType _cardType;
    [SerializeField] private List<ActionType> _actionList1;
    [SerializeField] private List<ActionType> _actionList2;
    [SerializeField] private List<ActionType> _actionList3;
    [SerializeField] private Sprite _cardImage;

    public CardType CardType => _cardType;
    public List<ActionType> ActionList1 => _actionList1;
    public List<ActionType> ActionList2 => _actionList2;
    public List<ActionType> ActionList3 => _actionList3;
    public Sprite CardImage => _cardImage;
}
