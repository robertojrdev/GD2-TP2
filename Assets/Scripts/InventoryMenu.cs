using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private Button agricultureButton;
    [SerializeField] private Button machineryButton;
    [SerializeField] private Button harvestedButton;
    [SerializeField] private Transform productsListHolder;
    [SerializeField] private InventoryListItem productsListItemPrefab;

    private ItemType currentType = ItemType.Agriculture;
    private Dictionary<ItemInfo, int> currentList;
    private List<InventoryListItem> listItems = new List<InventoryListItem>();

    private void Awake()
    {
        Inventory.onUpdateItemSupplyAmount += UpdateSupplyAmount;
        Inventory.onUpdateItemFinalProductAmount += UpdateFinalProductAmount;
    }

    private void Start()
    {
        agricultureButton.onClick.AddListener(() =>
        {
            SetSelectedButton(agricultureButton);
            ClearList();
            FillList(GameManager.Inventory.ItemsSupply, ItemType.Agriculture, true);
        });

        machineryButton.onClick.AddListener(() =>
        {
            SetSelectedButton(machineryButton);
            ClearList();
            FillList(GameManager.Inventory.ItemsSupply, ItemType.Machinery, true);
        });

        harvestedButton.onClick.AddListener(() =>
        {
            SetSelectedButton(harvestedButton);
            ClearList();
            FillList(GameManager.Inventory.ItemsFinalProduct, ItemType.Agriculture, false);
        });
    }

    private void OnDestroy()
    {
        Inventory.onUpdateItemSupplyAmount -= UpdateSupplyAmount;
        Inventory.onUpdateItemFinalProductAmount -= UpdateFinalProductAmount;
    }

    private void OnEnable()
    {
        if (currentList == null)
        {
            currentList = GameManager.Inventory.ItemsSupply;
            currentType = ItemType.Agriculture;
            SetSelectedButton(agricultureButton);
        }

        ClearList();
        FillList(currentList, currentType, currentList == GameManager.Inventory.ItemsSupply);
    }

    public void FillList(Dictionary<ItemInfo, int> itemList, ItemType type, bool canClickAtItems)
    {
        currentType = type;
        currentList = itemList;

        foreach (var inventoryItem in itemList)
        {
            if (inventoryItem.Key.type != type)
                continue;

            AddListItem(inventoryItem.Key, inventoryItem.Value, canClickAtItems);
        }
    }

    public void ClearList()
    {
        for (int i = listItems.Count - 1; i >= 0; i--)
        {
            RemoveListItem(listItems[i]);
        }
    }

    private void AddListItem(ItemInfo itemInfo, int amount, bool canClick)
    {
        var listItem = Instantiate(productsListItemPrefab);
        listItem.transform.SetParent(productsListHolder, false);

        listItem.SetItemInfo(itemInfo);
        listItem.UpdateAmount(amount);
        listItem.SetButtonEnabled(canClick);

        listItems.Add(listItem);

        if (canClick)
        {
            if (GameManager.Instance.itemToplace == itemInfo)
                listItem.SetSelected(true);
        }

        listItem.gameObject.SetActive(true);
    }

    private void RemoveListItem(InventoryListItem listItem)
    {
        listItems.Remove(listItem);
        Destroy(listItem.gameObject);
    }

    private void UpdateSupplyAmount(ItemInfo itemInfo, int amount)
    {
        if (currentList != GameManager.Inventory.ItemsSupply)
            return;

        var listItem = listItems.Find(x => x.itemInfo == itemInfo);

        if (!listItem)
        {
            if (amount > 0)
                AddListItem(itemInfo, amount, true);
        }
        else
        {
            if (amount <= 0)
                RemoveListItem(listItem);
            else
                listItem.UpdateAmount(amount);
        }
    }

    private void UpdateFinalProductAmount(ItemInfo itemInfo, int amount)
    {
        if (currentList != GameManager.Inventory.ItemsFinalProduct)
            return;

        var listItem = listItems.Find(x => x.itemInfo == itemInfo);

        if (!listItem)
        {
            if (amount > 0)
                AddListItem(itemInfo, amount, false);
        }
        else
        {
            if (amount <= 0)
                RemoveListItem(listItem);
            else
                listItem.UpdateAmount(amount);
        }
    }

    private void SetSelectedButton(Button selectedButton)
    {
        var buttons = new Button[] { agricultureButton, machineryButton, harvestedButton };

        for (int i = 0; i < buttons.Length; i++)
        {
            var img = buttons[i].GetComponent<Image>();
            if (buttons[i] == selectedButton)
                img.color = Color.green;
            else
                img.color = Color.white;
        }
    }
}