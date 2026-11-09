using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "Scriptable Objects/ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [SerializeField] private List<RoomData> _rooms;
    [SerializeField] private List<Vector2Int> _connectedRooms;

    public List<RoomData> Rooms => _rooms;
    public List<Vector2Int> ConnectedRooms => _connectedRooms;
}
