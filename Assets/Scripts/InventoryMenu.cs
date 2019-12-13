using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private Button agricultureButton;
    [SerializeField] private Button machineryButton;
    [SerializeField] private Transform productsListHolder;
    [SerializeField] private InventoryListItem productsListItemPrefab;

    private ItemType currentType = ItemType.Agriculture;
    private List<InventoryListItem> listItems = new List<InventoryListItem>();

    private void Awake()
    {
        Inventory.onUpdateItemAmount += UpdateAmount;
    }

    private void Start()
    {
        agricultureButton.onClick.AddListener(() =>
        {
            currentType = ItemType.Agriculture;
            ClearList();
            FillList();
        });

        machineryButton.onClick.AddListener(() =>
        {
            currentType = ItemType.Machinery;
            ClearList();
            FillList();
        });
    }

    private void OnDestroy()
    {
        Inventory.onUpdateItemAmount -= UpdateAmount;
    }

    private void OnEnable()
    {
        ClearList();
        FillList();
    }

    public void FillList()
    {
        foreach (var inventoryItem in GameManager.Inventory.Items)
        {
            if (inventoryItem.Key.type != currentType)
                continue;

            AddListItem(inventoryItem.Key, inventoryItem.Value);
        }
    }
    
    public void ClearList()
    {
        for (int i = listItems.Count -1; i >= 0; i--)
        {
            RemoveListItem(listItems[i]);
        }
    }

    private void AddListItem(ItemInfo itemInfo, int amount)
    {
        var listItem = Instantiate(productsListItemPrefab);
        listItem.transform.SetParent(productsListHolder, false);

        listItem.SetItemInfo(itemInfo);
        listItem.UpdateAmount(amount);

        listItems.Add(listItem);

        listItem.gameObject.SetActive(true);
    }

    private void RemoveListItem(InventoryListItem listItem)
    {
        listItems.Remove(listItem);
        Destroy(listItem.gameObject);
    }

    private void UpdateAmount(ItemInfo itemInfo, int amount)
    {
        var listItem = listItems.Find(x => x.itemInfo == itemInfo);

        if(!listItem)
        {
            if(amount > 0)
                AddListItem(itemInfo, amount);
        }
        else
        {
            if(amount <= 0)
                RemoveListItem(listItem);
            else
                listItem.UpdateAmount(amount);
        }
    }
}