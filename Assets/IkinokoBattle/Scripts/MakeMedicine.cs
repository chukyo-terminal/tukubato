using UnityEngine;
using UnityEngine.UI;

public class MakeMedicine : MonoBehaviour
{
    [SerializeField] private Button makeMeButton;
    [SerializeField] private ItemsDialog itemsDialog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        makeMeButton.onClick.AddListener(MakeMe);
    }

    public void MakeMe()
    {
        var owned = OwnedItemsData.Instance;

        const int needWood = 2;

        var wood = owned.GetItem(Item.ItemType.Wood);

        if (wood == null || wood.Number < needWood)
        {
            Debug.Log("‘fÞ‚ª‘«‚è‚Ü‚¹‚ñ");
            return;
        }

        owned.Use(Item.ItemType.Wood, needWood);

        owned.Add(Item.ItemType.Medicine, 1);

        owned.Save();

        itemsDialog.Rode();

        

        Debug.Log("–ò‚ðì¬");
    }
}
