using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    [SerializeField] private RoomType _roomType;
    [SerializeField] private string _name;
    [SerializeField] private ActionType _activateAction;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private CardType _craftingCard;

    public RoomType RoomType => _roomType;
    public string Name => _name;
    public ActionType ActivateAction => _activateAction;
    public ResourceType ResourceType => _resourceType;
    public CardType CraftingCard => _craftingCard;
}
