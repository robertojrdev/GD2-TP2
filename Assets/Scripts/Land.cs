using UnityEngine;

public class Land : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material acquiredMaterial;
    [SerializeField] private MeshRenderer meshRenderer;


    public bool Acquired { get; private set; }
    public Item Item { get; private set; }



    private void Awake()
    {
        meshRenderer.sharedMaterial = defaultMaterial;
        GameManager.onAdvanceSeason += AdvanceSeason;
    }

    private void OnDestroy()
    {
        GameManager.onAdvanceSeason -= AdvanceSeason;
    }

    public bool PlaceItem(ItemInfo itemInfo)
    {
        if (Item || !Acquired)
            return false;

        Item = Instantiate(itemInfo.item, transform.position, Quaternion.identity, transform);
        Item.SetInfo(itemInfo);
        return true;
    }

    /// <summary>
    /// Will remove the item if it is in planting state.
    /// </summary>
    /// <returns>Return true if the land become empty</returns>
    public bool RemoveItem()
    {
        if (!Item)
            return true;

        if (Item.stage == ItemStage.Planting)
        {
            Item.Destroy();
            Item = null;
            return true;
        }

        return false;
    }

    public void Acquire()
    {
        Acquired = true;
        meshRenderer.sharedMaterial = acquiredMaterial;
    }

    private void AdvanceSeason(Season season)
    {
        if (Item)
        {
            Item.AdvanceSeason(season);
        }
    }
}
