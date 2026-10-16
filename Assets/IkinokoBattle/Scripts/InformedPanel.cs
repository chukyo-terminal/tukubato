using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static OwnedItemsData;

public class InformedPanel : MonoBehaviour
{
    [SerializeField] private PlayerStatus player;
    [SerializeField] private GameObject informedPanel;
    [SerializeField] private Button yes;
    [SerializeField] private GameObject yesO;
    [SerializeField] private Button no;
    [SerializeField] private ItemButton currentButton;
    [SerializeField] private GameObject itemButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        informedPanel.SetActive(false);
        yes.onClick.AddListener(YesMe);
        no.onClick.AddListener(NoMe);
    }

    public void Open(ItemButton button)
    {
        currentButton = button;
        informedPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(yesO);
    }
    public void YesMe()
    {
        var owned = OwnedItemsData.Instance;

        player.Heal(30);

        OwnedItemsData.Instance.Use(Item.ItemType.Medicine, 1);
        currentButton.OwnedItem = OwnedItemsData.Instance.GetItem(Item.ItemType.Medicine);
        informedPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(itemButton);
        PlayerEvents.OnUseItem?.Invoke();
    }

    public void NoMe()
    {
        informedPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(itemButton);
    }
}
