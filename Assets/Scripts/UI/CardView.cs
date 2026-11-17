using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image _selecteCardIamge;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Transform _actionListView;

    public Image SelecteCardIamge => _selecteCardIamge;
    public Image CardImage => _cardImage;
    public Transform ActionListView => _actionListView;

    public void SetCardData(PlayCardData cardData)
    {
        _cardImage.sprite = cardData.CardImage;
    }

    public void Select()
    {
        _selecteCardIamge.gameObject.SetActive(true);
    }
}
