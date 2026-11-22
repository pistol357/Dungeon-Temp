using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] RoomType _roomType;

    private int _roomNumber;
    private string _name;
    private List<Monster> _monsters = new();
    private List<Hero> _heroes = new();
    private Dictionary<ResourceType, int> _resources = new();
    private Treasure _treasure;

    public int RoomNumber => _roomNumber;
    public string Name => _name;
    public ActionType ActivateAction => _roomData.ActivateAction;
    public ResourceType ResourceType => _roomData.ResourceType;
    public CardType CraftCard => _roomData.CraftingCard;
    public RoomType RoomType => _roomType;
    public List<Monster> Monsters => _monsters;
    public List<Hero> Heroes => _heroes;
    public Dictionary<ResourceType, int> Resources => _resources;
    public Treasure Treasure => _treasure;

    public Room(RoomData data, int num, TreasureData treasureData)
    {
        _roomData = data;
        _roomNumber = num;
        _name = data.Name;
        _roomType = data.RoomType;
        _treasure = new Treasure(treasureData);
    }

    public void AddUnit(IActionable unit)
    {
        if(unit is Monster)
        {
            _monsters.Add((unit as Monster));
        }
        else if(unit is Hero)
        {
            _heroes.Add((unit as Hero));
        }
    }

    public void RemoveUnit(IActionable unit)
    {
        if (_monsters.Contains((unit as Monster)))
        {
            _monsters.Remove((unit as Monster));
            return;
        }
        else if(_heroes.Contains((unit as Hero)))
        {
            _heroes.Remove((unit as Hero));
            return;
        }

        Debug.Log($"{unit}(이)가 {_name}에 존재하지 않음");
    }

    public void AddResource(ResourceType resourceType)
    {
        if (resourceType == ResourceType.None) return;

        if (_resources.ContainsKey(resourceType)) _resources[resourceType]++;
        else _resources.Add(resourceType, 1);
    }

    public void RemoveResource(ResourceType resourceType)
    {
        if (_resources.ContainsKey(resourceType) && _resources[resourceType] > 0)
        {
            _resources[resourceType]--;

            if (_resources[resourceType] == 0) _resources.Remove(resourceType);
        }
    }

    public bool IsTrapExist()
    {
        return _resources.ContainsKey(ResourceType.Trap);
    }
}
