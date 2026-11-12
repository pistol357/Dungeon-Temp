using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
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

    public void Summon(int roomNumber, IActionable unit)
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(roomNumber);

        room.AddUnit(unit);
        unit.SetRoom(room);
        Debug.Log($"{unit}(이)가 {room.Name}에 매복함");
    }

    public void Move(IActionable unit, int roomNumber, Resource resource)
    {
        int currentRoomNumber = unit.CurrentRoomNumber;
        Room currentRoom = DungeonMap.Instance.GetRoomByNumber(currentRoomNumber);
        Room nextRoom = DungeonMap.Instance.GetRoomByNumber(roomNumber);

        currentRoom.RemoveUnit(unit);
        nextRoom.AddUnit(unit);
        unit.SetRoom(nextRoom);
        Debug.Log($"{unit}(이)가 {currentRoom.Name}에서 {nextRoom.Name}(으)로 이동함");
    }
}
