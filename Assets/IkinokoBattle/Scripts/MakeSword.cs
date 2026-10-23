using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class MakeSword : MonoBehaviour
{
    [SerializeField] private Button makeSwordButton;
    [SerializeField] private ItemsDialog itemsDialog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        makeSwordButton.onClick.AddListener(MakeSw);
    }


    public void MakeSw()
    {
        var owned = OwnedItemsData.Instance;

        const int needWood = 2;
        const int needStone = 2;

        var wood = owned.GetItem(Item.ItemType.Wood);
        var stone = owned.GetItem(Item.ItemType.Stone);

        if (wood == null || wood.Number < needWood || stone == null || stone.Number < needStone)
        {
            Debug.Log("‘fÞ‚ª‘«‚è‚Ü‚¹‚ñ");
            return;
        }

        owned.Use(Item.ItemType.Wood, needWood);
        owned.Use(Item.ItemType.Stone, needStone);

        owned.Add(Item.ItemType.Sword, 1);

        owned.Save();

        itemsDialog.Rode();

        Debug.Log("Œ•‚ðì¬");
    }
}