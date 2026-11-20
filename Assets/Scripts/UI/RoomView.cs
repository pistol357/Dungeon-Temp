using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private Transform _monsterPosition;
    [SerializeField] private Transform _heroPosition;
    [SerializeField] private UnitIcon _unitPrefab;

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        Room room = GetRoom();

        foreach(Transform child in _monsterPosition)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _heroPosition)
        {
            Destroy(child.gameObject);
        }

        foreach (Monster monster in room.Monsters)
        {
            UnitIcon monsterIcon = Instantiate(_unitPrefab, _monsterPosition);
            monsterIcon.SetIcon(monster.Icon);

            if (_clan.State == WaitFor.SelectMonster && _clan.CurrentMonster == monster) monsterIcon.Select();
        }

        foreach (Hero hero in room.Heroes)
        {
            UnitIcon heroIcon = Instantiate(_unitPrefab, _heroPosition);
            heroIcon.SetIcon(hero.Icon);

            if (_clan.State == WaitFor.SelectTargetHero && _clan.CurrentHero == hero) heroIcon.Select();

            if (hero.State == UnitState.Exhausted)
            {
                heroIcon.Exhaust();
            }
        }

        Color color = GetComponentInChildren<Image>().color;
        if ((_clan.State == WaitFor.SelectMoveRoom || _clan.State == WaitFor.SelectSummonRoom))
        {
            if (_roomNumber == _clan.CurrentRoomNumber)
            {
                color.a = 1f;
                GetComponentInChildren<Image>().color = color;
                return;
            }

            for(int i = 0; i < _clan.CanSelectRoomNumber.Count; i++)
            {
                if (_roomNumber == _clan.CanSelectRoomNumber[i])
                {
                    color.a = 0.4f;
                    GetComponentInChildren<Image>().color = color;
                    return;
                }
            }

            color.a = 0.05f;
            GetComponentInChildren<Image>().color = color;
        }
        else
        {
            color.a = 1f;
            GetComponentInChildren<Image>().color = color;
        }

        if ((_clan.State == WaitFor.SelectMonster || _clan.State == WaitFor.SelectTargetHero))
        {
            color.a = 0.05f;
            GetComponentInChildren<Image>().color = color;
        }
    }

    public Room GetRoom()
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(_roomNumber);

        return room;
    }
}
