using UnityEngine;

public class ItemsDialog : MonoBehaviour
{
    [SerializeField] private int buttonNumber = 15;
    [SerializeField] private ItemButton itemButton;

    private ItemButton[] _itemButtons;
    public bool IsOpen { get; private set; }

    private void Start()
    {
        gameObject.SetActive(false);

        for (var i = 0; i < buttonNumber - 1; i++)
        {
            Instantiate(itemButton, transform);
        }

        _itemButtons = GetComponentsInChildren<ItemButton>();
    }

    public void Open()
    {
        IsOpen = true;
        gameObject.SetActive(true);

        for (var i = 0; i < buttonNumber; i++)
        {
            _itemButtons[i].OwnedItem = OwnedItemsData.Instance.OwnedItems.Length > i
                ? OwnedItemsData.Instance.OwnedItems[i]
                : null;
        }
    }

    public void Rode()
    {
        for (var i = 0; i < buttonNumber; i++)
        {
            _itemButtons[i].OwnedItem = OwnedItemsData.Instance.OwnedItems.Length > i
                ? OwnedItemsData.Instance.OwnedItems[i]
                : null;
        }
    }
    public void Close()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
