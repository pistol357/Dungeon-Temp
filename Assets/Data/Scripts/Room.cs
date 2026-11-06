using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    [SerializeField] private RoomType _roomType;
    [SerializeField] private string _name;
    [SerializeField] private string _ability;

    public RoomType RoomType => _roomType;
    public string Name => _name;
    public string Ability => _ability;
}
