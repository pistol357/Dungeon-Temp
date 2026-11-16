using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Image _countImage;
    [SerializeField] private List<Sprite> _countImages;

    public ResourceType ResourceType => _resourceType;

    public void RefreshUI(int count)
    {
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
}
