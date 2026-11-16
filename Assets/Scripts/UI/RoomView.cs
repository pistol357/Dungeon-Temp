using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private Transform _monsterPosition;
    [SerializeField] private Transform _heroPosition;
    [SerializeField] private Image _unitPrefab;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        Room room = DungeonMap.Instance.GetRoomByNumber(_roomNumber);

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
    }
}
