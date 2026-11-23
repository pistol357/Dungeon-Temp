using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private PlayCardData _cardData;
    [SerializeField] private Image _selecteCardIamge;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Transform _actionListView;
    [SerializeField] private GameObject _actionIconPrefab;

    public void SetCardData(PlayCardData cardData)
    {
        _cardData = cardData;

        if(cardData == null)
        {
            _cardImage.enabled = false;
            _actionListView.gameObject.GetComponent<Image>().enabled = false;
            foreach (Transform child in _actionListView)
            {
                Destroy(child.gameObject);
            }
            return;
        }
        _cardImage.sprite = cardData.CardImage;
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in _actionListView)
        {
            Destroy(child.gameObject);
        }

        if (_cardData.ActionList1 != null && _cardData.ActionList1.Count > 0)
        {
            foreach (ActionType action in _cardData.ActionList1)
            {
                Image actionIcon = Instantiate(_actionIconPrefab, _actionListView).GetComponentInChildren<Image>();
                actionIcon.sprite = UIManager.Instance.GetSpriteByActionType(action);
            }
        }

        if (_cardData.ActionList2 != null && _cardData.ActionList2.Count > 0)
        {
            Image slash = Instantiate(_actionIconPrefab, _actionListView).GetComponentInChildren<Image>();
            slash.sprite = UIManager.Instance.GetSpriteByActionType(ActionType.None);

            foreach (ActionType action in _cardData.ActionList2)
            {
                Image actionIcon = Instantiate(_actionIconPrefab, _actionListView).GetComponentInChildren<Image>();
                actionIcon.sprite = UIManager.Instance.GetSpriteByActionType(action);
            }
        }

        if (_cardData.ActionList3 != null && _cardData.ActionList3.Count > 0)
        {
            Image slash = Instantiate(_actionIconPrefab, _actionListView).GetComponentInChildren<Image>();
            slash.sprite = UIManager.Instance.GetSpriteByActionType(ActionType.None);

            foreach (ActionType action in _cardData.ActionList3)
            {
                Image actionIcon = Instantiate(_actionIconPrefab, _actionListView).GetComponentInChildren<Image>();
                actionIcon.sprite = UIManager.Instance.GetSpriteByActionType(action);
            }
        }
    }

    public void Select()
    {
        _selecteCardIamge.gameObject.SetActive(true);
    }
}
