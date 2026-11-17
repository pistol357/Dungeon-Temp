using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureSlot : MonoBehaviour
{
    [SerializeField] private Image _valueImage;
    [SerializeField] private List<Sprite> _valueImages;

    private RoomView _roomView;

    private void Awake() => CacheComponents();
    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        Room room = _roomView.GetRoom();

        if (room.Treasure.IsOpened)
        {
            gameObject.SetActive(false);
            return;
        }

        _valueImage.sprite = _valueImages[room.Treasure.Value];
    }

    private void CacheComponents()
    {
        _roomView = GetComponentInParent<RoomView>();
    }
}
