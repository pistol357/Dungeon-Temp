using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] RoomType _roomType;

    private int _roomNumber;
    private string _name;
    private bool _hasTreasure;
    private List<Monster> _monsters = new();
    private List<Resource> _resources = new();
    private int _TrapCount;

    public int RoomNumber => _roomNumber;
    public string Name => _name;
    public RoomType RoomType => _roomType;
    public List<Monster> Monsters => _monsters;
    public List<Resource> Resources => _resources;

    public Room(RoomData data, int num)
    {
        _roomData = data;
        _roomNumber = num;
        _name = data.Name;
        _roomType = data.RoomType;
    }

    public void AddMonster(Monster monster)
    {
        _monsters.Add(monster);
    }
}
