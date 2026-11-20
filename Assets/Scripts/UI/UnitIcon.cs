using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Image _unitIcon;

    public void SetIcon(Sprite sprite)
    {
        _unitIcon.sprite = sprite;
    }

    public void Active()
    {
        _unitIcon.color = Color.white;
    }

    public void Exhaust()
    {
        _unitIcon.color = Color.grey;
    }

    public void Select()
    {
        _selectedImage.enabled = true;
    }

    public void Unselect()
    {
        _selectedImage.enabled = false;
    }
}
