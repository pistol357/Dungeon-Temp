using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private Image _countImage;
    [SerializeField] private List<Sprite> _countImages;

    private RoomView _roomView;

    public ResourceType ResourceType => _resourceType;

    private void Awake() => CacheComponents();
    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        int count = 0;

        if (_roomView.GetRoom().Resources.ContainsKey(_resourceType)) count = _roomView.GetRoom().Resources[_resourceType];

        if (count == 0)
        {
            _resourceIcon.enabled = false;
            _countImage.enabled = false;
        }
        else if (count > 0)
        {
            _resourceIcon.enabled = true;
            _countImage.enabled = true;
            _countImage.sprite = _countImages[count];
        }
    }

    private void CacheComponents()
    {
        _roomView = GetComponentInParent<RoomView>();
    }
}
