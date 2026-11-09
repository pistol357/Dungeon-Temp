using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    [SerializeField] private RoomType _roomType;
    [SerializeField] private string _name;
    [SerializeField] private RoomActivateType _activate;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private CardType _craftingCard;

    public RoomType RoomType => _roomType;
    public string Name => _name;
    public RoomActivateType Activate => _activate;
    public ResourceType ResourceType => _resourceType;
    public CardType CraftingCard => _craftingCard;
}
