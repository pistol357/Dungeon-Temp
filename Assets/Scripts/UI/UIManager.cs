using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _actionIcon;
    [SerializeField] private List<Sprite> _numberIcon;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>();
            }

            return _instance;
        }
    }

    private void Awake() => SetSingleton();

    public Sprite GetSpriteByActionType(ActionType actionType)
    {
        return _actionIcon[(int)actionType];
    }

    public Sprite GetSpriteByNumber(int number)
    {
        return _numberIcon[number];
    }

    private void SetSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
