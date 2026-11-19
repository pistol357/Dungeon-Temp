using UnityEngine;
using UnityEngine.UI;

public class ActionIconView : MonoBehaviour
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Image _actionIcon;

    public void SetIcon(Sprite sprite)
    {
        _actionIcon.sprite = sprite;
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
