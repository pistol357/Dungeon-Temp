using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Image _countImage;
    [SerializeField] private List<Sprite> _countImages;

    private RoomView _roomView;

    public ResourceType ResourceType => _resourceType;

    private void Awake() => CacheComponents();
    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        List<Resource> resources = _roomView.GetRoom().Resources.ToList();

        int count = 0;
        foreach(Resource resource in resources)
        {
            if (resource.Type == _resourceType)
            {
                count++;
            }
        }

        if (count == 0)
        {
            gameObject.SetActive(false);
        }
        else if (count > 0)
        {
            gameObject.SetActive(true);
            _countImage.sprite = _countImages[count];
        }
    }

    private void CacheComponents()
    {
        _roomView = GetComponentInParent<RoomView>();
    }
}
