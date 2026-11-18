using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private Transform _monsterPosition;
    [SerializeField] private Transform _heroPosition;
    [SerializeField] private Image _unitPrefab;

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
            Instantiate(_unitPrefab, _monsterPosition).sprite = monster.Icon;
        }

        foreach (Hero hero in room.Heroes)
        {
            Image heroImage = Instantiate(_unitPrefab, _heroPosition);
            heroImage.sprite = hero.Icon;

            if (hero.State == UnitState.Exhausted)
            {
                heroImage.color = Color.grey;
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
    }

    public Room GetRoom()
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(_roomNumber);

        return room;
    }
}
