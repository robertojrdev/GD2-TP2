using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    #region Static Getters
    public static Market Market { get => instance?.market; }
    public static Inventory Inventory { get => instance?.inventory; }
    public static Season CurrentSeason { get => instance ? instance.currentSeason : Season.Autumn; }
    #endregion

    #region Inspector Variables
    [SerializeField] private InitialSetup initialSetup;
    #endregion

    #region Private Variables
    private Market market = new Market();
    private Inventory inventory = new Inventory();
    private Season currentSeason = Season.Autumn;
    public ItemInfo itemToplace { get; private set; }

    private Camera _camera;
    #endregion

    #region Events
    /// <summary>
    /// This will be called to calculate player actions
    /// </summary>
    public static event Action<Season> onAdvanceSeason;
    /// <summary>
    /// This will be called after onAdvanceSeason when all the players actions are computed.
    /// <para>ex: Update season icon.</para>
    /// </summary>
    public static event Action<Season> onNewSeason;
    #endregion

    #region Static Funcions
    public static void SetItemToPlace(ItemInfo itemInfo)
    {
        if (instance)
            instance.itemToplace = itemInfo;
    }
    public static void NextSeason()
    {
        if (!instance)
            return;

        instance.currentSeason = (Season)(((int)instance.currentSeason + 1) % 4);

        if (onAdvanceSeason != null)
            onAdvanceSeason.Invoke(instance.currentSeason);

        if (onNewSeason != null)
            onNewSeason.Invoke(instance.currentSeason);
    }
    #endregion

    #region Private Functions
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Level.Instance.DrawLevel(initialSetup.mapSize, initialSetup.acquiredLands);
        onNewSeason += season => market.ResetMovements();
        _camera = Camera.main;
        InitialResourcesSetup();

        Market.onPurchase += (market, mProduct, amount) => Log.Msg(
            "Purchased " + amount + " of " + mProduct.product.name, LogType.Info);

        Market.onSell += (market, mProduct, amount) => Log.Msg(
            "Sold " + amount + " of " + mProduct.product.name, LogType.Info);
    }

    private void InitialResourcesSetup()
    {
        //Set initial values
        foreach (var item in initialSetup.initialItems)
        {
            inventory.AddItemSupply(item.itemInfo, item.amount);
        }

        inventory.AddMoney(initialSetup.money);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            if (itemToplace != null && inventory.GetItemSupplyAmount(itemToplace) > 0)
                PlaceItem(itemToplace);
        if (Input.GetMouseButton(1))
            RemoveItem();
    }

    private void PlaceItem(ItemInfo info)
    {
        Land land;
        if (RaycastLand(out land))
        {
            var placed = land.PlaceItem(info);
            if (placed)
            {
                inventory.RemoveItemSupply(info);
            }
            else
            {
                //do whatever when cannot place on this land
            }
        }
    }

    private void RemoveItem()
    {
        Land land;
        if (RaycastLand(out land))
        {
            if (land.Item != null)
            {
                inventory.AddItemSupply(land.Item.itemInfo);
                land.RemoveItem();
            }
        }
    }

    private bool RaycastLand(out Land outLand)
    {
        outLand = null;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Land")))
        {
            var land = hit.transform.GetComponent<Land>();
            if (land != null)
            {
                outLand = land;
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Public Functions

    #endregion
}