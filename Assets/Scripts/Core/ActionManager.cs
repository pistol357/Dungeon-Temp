using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager _instance;

    public static ActionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ActionManager>();
            }

            return _instance;
        }
    }

    private void Awake() => SetSingleton();

    public RoomType ActionTypeToRoomType(ActionType action, IActionable unit)
    {
        if (action == ActionType.SummonInEye)
        {
            return RoomType.Eye;
        }
        else if (action == ActionType.SummonInHat)
        {
            return RoomType.Hat;
        }
        else if (action == ActionType.SummonInJail)
        {
            return RoomType.Jail;
        }
        else if (action == ActionType.SummonInSameRoom)
        {
            return DungeonMap.Instance.GetRoomTypeByNumber(unit.CurrentRoomNumber);
        }
        else if (action == ActionType.SummonInSkull)
        {
            return RoomType.Skull;
        }
        else if (action == ActionType.SummonInSword)
        {
            return RoomType.Sword;
        }
        else if (action == ActionType.SummonInTreasure)
        {
            return RoomType.Treasure;
        }
        else
        {
            return RoomType.None;
        }
    }

    public ActionType RoomTypeToActionType(RoomType roomType)
    {
        if(roomType == RoomType.Eye)
        {
            return ActionType.SummonInEye;
        }
        else if (roomType == RoomType.Hat)
        {
            return ActionType.SummonInHat;
        }
        else if (roomType == RoomType.Jail)
        {
            return ActionType.SummonInJail;
        }
        else if (roomType == RoomType.Skull)
        {
            return ActionType.SummonInSkull;
        }
        else if (roomType == RoomType.Sword)
        {
            return ActionType.SummonInSword;
        }
        else if (roomType == RoomType.Treasure)
        {
            return ActionType.SummonInTreasure;
        }
        else
        {
            return ActionType.None;
        }
    }

    private ResourceType ActionTypeToResourceType(ActionType action)
    {
        if (action == ActionType.CreateBone)
        {
            return ResourceType.Bone;
        }
        else if (action == ActionType.CreateBook)
        {
            return ResourceType.Book;
        }
        else if (action == ActionType.CreateCoin)
        {
            return ResourceType.Coin;
        }
        else if (action == ActionType.CreateFrog)
        {
            return ResourceType.Frog;
        }
        else if (action == ActionType.CreateTrap)
        {
            return ResourceType.Trap;
        }
        else
        {
            return ResourceType.None;
        }
    }

    public void Summon(int roomNumber, IActionable unit)
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

        room.AddUnit(unit);
        unit.SetRoom(room);

        Debug.Log($"{unit}(이)가 {room.Name}에 매복함");
    }

    public void Return(int roomNumber, IActionable unit)
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

        room.RemoveUnit(unit);
        unit.SetRoom(null);
    }

    public void Move(IActionable unit, int roomNumber, Resource resource)
    {
        int currentRoomNumber = unit.CurrentRoomNumber;
        Room currentRoom = DungeonMap.Instance.GetRoomByNumber(currentRoomNumber);
        Room nextRoom = DungeonMap.Instance.GetRoomByNumber(roomNumber);

        currentRoom.RemoveUnit(unit);
        currentRoom.RemoveResource(resource);
        nextRoom.AddUnit(unit);
        nextRoom.RemoveResource(resource);
        unit.SetRoom(nextRoom);

        if (resource == null) Debug.Log($"{unit}(이)가 {currentRoom.Name}에서 {nextRoom.Name}(으)로 이동함");
        else Debug.Log($"{unit}(이)가 {currentRoom.Name}에서 {resource}를 가지고 {nextRoom.Name}(으)로 이동함");
    }

    public void Create(IActionable unit, ActionType action)
    {
        int currentRoomNumber = unit.CurrentRoomNumber;
        Room room = DungeonMap.Instance.GetRoomByNumber(currentRoomNumber);
        ResourceType resourceType = ActionTypeToResourceType(action);

        Resource resource = new Resource(resourceType);
        room.AddResource(resource);

        Debug.Log($"{resource.Type}을 {room.Name}에 생산함");
    }

    private void SetSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
