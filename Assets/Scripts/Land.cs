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
    }

    public bool PlaceItem(ItemInfo itemInfo)
    {
        if (Item || !Acquired)
            return false;

        Item = Instantiate(itemInfo.item, transform.position, Quaternion.identity, transform);
        Item.SetInfo(itemInfo);
        return true;
    }

    public void RemoveItem()
    {
        if (!Item)
            return;

        Item.Destroy();
        Item = null;
    }

    public void Acquire()
    {
        Acquired = true;
        meshRenderer.sharedMaterial = acquiredMaterial;
    }

}
