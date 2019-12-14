using UnityEngine;
using UnityEngine.UI;

public class BasicInfoMenu : MonoBehaviour
{
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text seasonText;
    [SerializeField] private Image seasonImage;
    [SerializeField] private Text expensesText;

    private void Awake()
    {
        nextTurnButton.onClick.AddListener(GameManager.NextSeason);
        GameManager.onNewSeason += UpdateSeason;
        Inventory.onUpdateMoney += UpdateMoney;
        UpdateSeason(Season.Autumn);
    }

    private void OnDestroy()
    {
        GameManager.onNewSeason -= UpdateSeason;
        Inventory.onUpdateMoney -= UpdateMoney;
    }

    private void UpdateMoney(int amount) => moneyText.text = string.Format("${0:00.00}", amount);
    private void UpdateSeason(Season season)
    {
        seasonText.text = season.ToString();
        var iconName = "icon-season-" + season.ToString();
        seasonImage.sprite = SpritesCatalog.Get(iconName);
    }
}