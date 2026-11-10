using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] private int _roomNumber;
    [SerializeField] RoomType _roomType;

    private bool _hasTreasure;
    private List<Resource> _resources;
    private int _TrapCount;

    public RoomType RoomType => _roomType;

    public Room(RoomData data, int num)
    {
        _roomData = data;
        _roomNumber = num;
        _roomType = data.RoomType;
    }
}
