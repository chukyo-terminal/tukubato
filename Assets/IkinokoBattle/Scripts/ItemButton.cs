using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
	public OwnedItemsData.OwnedItem OwnedItem
	{
		get { return _ownedItem; }
		set
		{
			_ownedItem = value;

			var isEmpty = null == _ownedItem;
			image.gameObject.SetActive(!isEmpty);
			number.gameObject.SetActive(!isEmpty);
			_button.interactable = !isEmpty;
			if(!isEmpty)
			{
				image.sprite = itemSprites.First(x => x.itemType == _ownedItem.Type).sprite;
				number.text = "" + _ownedItem.Number;
			}
		}
	}

	[SerializeField] private ItemTypeSpriteMap[] itemSprites;
	[SerializeField] private Image image;
	[SerializeField] private Text number;
	[SerializeField] private InformedPanel infPanel;

	private Button _button;
	private OwnedItemsData.OwnedItem _ownedItem;
	

    private void Awake()
    {
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (_ownedItem == null) return;

        switch (_ownedItem.Type)
        {
            case Item.ItemType.Medicine:
            case Item.ItemType.Sword:
            case Item.ItemType.Spear:
                infPanel.Open(this);
                break;

            default:
                // それ以外は何もしない
                break;
        }
    }


    [Serializable]
	public class ItemTypeSpriteMap
	{
		public Item.ItemType itemType;
		public Sprite sprite;
	}
}
