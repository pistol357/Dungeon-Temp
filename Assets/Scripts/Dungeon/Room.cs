using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomData _roomData;
    
    private Treasure _treasure;

    public Room(RoomData data)
    {
        _roomData = data;
    }
}
