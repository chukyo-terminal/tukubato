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

    [SerializeField] private Text infoText;

    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private WeaponData sword;
    [SerializeField] private WeaponData spear;

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
        switch (button.OwnedItem.Type)
        {
            case Item.ItemType.Medicine:
                infoText.text = "薬を使いますか？";
                break;

            case Item.ItemType.Sword:
                infoText.text = "剣を装備しますか？";
                break;

            case Item.ItemType.Spear:
                infoText.text = "槍を装備しますか？";
                break;

            default:
                infoText.text = "このアイテムを使いますか？";
                break;
        }
        EventSystem.current.SetSelectedGameObject(yesO);
    }
    public void YesMe()
    {
        var owned = OwnedItemsData.Instance;

        switch (currentButton.OwnedItem.Type)
        {
            case Item.ItemType.Medicine:
                player.Heal(30);
                OwnedItemsData.Instance.Use(currentButton.OwnedItem.Type, 1);
                break;

            case Item.ItemType.Sword:
                weaponController.EquipWeapon(sword);   // 例：装備処理
                break;

            case Item.ItemType.Spear:
                weaponController.EquipWeapon(spear);   // 例：装備処理
                break;
        }       

        currentButton.OwnedItem = OwnedItemsData.Instance.GetItem(currentButton.OwnedItem.Type);
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
