using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "Scriptable Objects/ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [SerializeField] private List<RoomData> _rooms;
    [SerializeField] private List<Vector2Int> _connectedRooms;
    [SerializeField] private List<TreasureData> _treasures;
    [SerializeField] private List<Vector2Int> _roomToTreasure;

    public List<RoomData> Rooms => _rooms;
    public List<Vector2Int> ConnectedRooms => _connectedRooms;
    public List<TreasureData> Treasures => _treasures;
    public List<Vector2Int> RoomToTreasure => _roomToTreasure;
}
