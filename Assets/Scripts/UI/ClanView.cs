using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanView : MonoBehaviour
{
    [SerializeField] private Image _clanMonsterIcon;
    [SerializeField] private List<Image> _unitCountImage = new();
    [SerializeField] private List<Image> _healthImage = new();
    [SerializeField] private List<Image> _deckCountImage = new();

    private Clan _clan => TurnManager.Instance.CurrentTurnClan;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        _clanMonsterIcon.sprite = _clan.MonsterIcon;

        _unitCountImage[0].sprite = UIManager.Instance.GetSpriteByNumber(_clan.ActiveMonsterCount);
        _unitCountImage[1].sprite = UIManager.Instance.GetSpriteByNumber(_clan.MonsterCount);

        _healthImage[0].sprite = UIManager.Instance.GetSpriteByNumber(_clan.Hp);
        _healthImage[1].sprite = UIManager.Instance.GetSpriteByNumber(_clan.MaxHp);

        _deckCountImage[0].sprite = UIManager.Instance.GetSpriteByNumber(_clan.RemainDeckCount / 10);
        _deckCountImage[1].sprite = UIManager.Instance.GetSpriteByNumber(_clan.RemainDeckCount % 10);
        _deckCountImage[2].sprite = UIManager.Instance.GetSpriteByNumber(_clan.DeckCount / 10);
        _deckCountImage[3].sprite = UIManager.Instance.GetSpriteByNumber(_clan.DeckCount % 10);
    }
}
