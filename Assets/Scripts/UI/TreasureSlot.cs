using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureSlot : MonoBehaviour
{
    [SerializeField] private Image _valueImage;
    [SerializeField] private List<Sprite> _valueImages;

    public void SetValue(int value)
    {
        _valueImage.sprite = _valueImages[value];

        if (value > 0)
        {
            gameObject.SetActive(true);
        }
    }

    public void Open()
    {
        SetValue(0);
        gameObject.SetActive(false);
    }
}
