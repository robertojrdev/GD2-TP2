using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    #region Static Getters
    public static Inventory Inventory { get => instance?.inventory; }
    public static Season CurrentSeason { get => instance ? instance.currentSeason : Season.Autumn; }
    #endregion

    #region Inspector Variables
    [SerializeField] private InitialSetup initialSetup;
    #endregion

    #region Private Variables
    private Inventory inventory = new Inventory();
    private Season currentSeason = Season.Autumn;
    private ItemInfo itemToplace;

    private Camera _camera;
    #endregion

    #region Events
    public static event Action<Season> onNextSeason;
    #endregion

    #region Static Funcions
    public static void SetItemToPlace(ItemInfo itemInfo)
    {
        if(instance)
            instance.itemToplace = itemInfo;
    }
    public static void NextSeason()
    {
        if(!instance)
            return;

        instance.currentSeason = (Season)(((int)instance.currentSeason + 1) % 4);

        if(onNextSeason != null)
            onNextSeason.Invoke(instance.currentSeason);
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
        _camera = Camera.main;
        Level.Instance.DrawLevel(initialSetup.mapSize, initialSetup.acquiredLands);

        var acquiredPos = new Vector3(initialSetup.acquiredLands.x, 0, initialSetup.acquiredLands.y);
        _camera.transform.position = Level.Center - acquiredPos + Vector3.up * 5f;

        InitialResourcesSetup();
    }

    private void InitialResourcesSetup()
    {
        //Set initial values
        foreach (var item in initialSetup.initialItems)
        {
            inventory.Add(item.itemInfo, item.amount);
        }

        inventory.AddMoney(initialSetup.money);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            if (itemToplace != null && inventory.GetItemAmount(itemToplace) > 0)
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
                inventory.Remove(info);
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
                inventory.Add(land.Item.itemInfo);
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