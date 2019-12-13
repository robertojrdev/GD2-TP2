using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryListItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text amountText;

    
    private Button _button;

    public ItemInfo itemInfo { get; private set; }


    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => GameManager.SetItemToPlace(itemInfo));
    }

    public void SetItemInfo(ItemInfo info)
    {
        icon.sprite = info.icon;
        itemName.text = info.name;
        itemInfo = info;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}