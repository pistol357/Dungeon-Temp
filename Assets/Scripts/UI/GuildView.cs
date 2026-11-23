using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildView : MonoBehaviour
{
    [SerializeField] private Guild _guild;
    [SerializeField] private Image _waveCount;
    [SerializeField] private List<Image> _deckCountImage = new();
    [SerializeField] private List<CardView> _craftCardImage = new();

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        _waveCount.sprite = UIManager.Instance.GetSpriteByNumber(_guild.Wave);

        _deckCountImage[0].sprite = UIManager.Instance.GetSpriteByNumber(_guild.RemainDeckCount / 10);
        _deckCountImage[1].sprite = UIManager.Instance.GetSpriteByNumber(_guild.RemainDeckCount % 10);
        _deckCountImage[2].sprite = UIManager.Instance.GetSpriteByNumber(_guild.DeckCount / 10);
        _deckCountImage[3].sprite = UIManager.Instance.GetSpriteByNumber(_guild.DeckCount % 10);

        _craftCardImage[0].SetCardData(DungeonMap.Instance.GetCardData(CardType.Equipment));
        _craftCardImage[1].SetCardData(DungeonMap.Instance.GetCardData(CardType.Beast));
        _craftCardImage[2].SetCardData(DungeonMap.Instance.GetCardData(CardType.Scroll));
        _craftCardImage[3].SetCardData(DungeonMap.Instance.GetCardData(CardType.Potion));
    }
}
