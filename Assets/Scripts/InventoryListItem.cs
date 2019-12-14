using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryListItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text amountText;
    [SerializeField] private Color selectedColor = Color.yellow;


    private Button _button;
    private Image _image;
    private Color _defaultColor;

    public ItemInfo itemInfo { get; private set; }


    private void Awake()
    {
        GetButton();
        GetImage();

        _button.onClick.AddListener(() => GameManager.SetItemToPlace(itemInfo));
        _button.onClick.AddListener(() => SetSelected(true));
    }

    public void SetItemInfo(ItemInfo info)
    {
        icon.sprite = info.icon;
        itemName.text = info.name;
        itemInfo = info;
    }

    public void SetButtonEnabled(bool enabled)
    {
        GetButton();
        _button.enabled = enabled;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void SetSelected(bool selected)
    {
        GetImage();
        _image.color = selected ? selectedColor : _defaultColor;

        if (selected)
        {
            var group = transform.parent.GetComponent<ListItemGroup>();
            if (group == null)
            {
                group = transform.parent.gameObject.AddComponent(typeof(ListItemGroup)) as ListItemGroup;
            }

            group.SetSelected(this);
        }
    }

    private void GetImage()
    {
        if (_image)
            return;

        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    private void GetButton()
    {
        if (_button)
            return;

        _button = GetComponent<Button>();
    }
}