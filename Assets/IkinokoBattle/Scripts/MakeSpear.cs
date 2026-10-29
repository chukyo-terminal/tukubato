using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class MakeSpear : MonoBehaviour
{
    [SerializeField] private Button makeSpearButton;
    [SerializeField] private ItemsDialog itemsDialog;
    [SerializeField] private CraftConfirmPanel craftPanel;
    [SerializeField] private CraftRecipe recipe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        makeSpearButton.onClick.AddListener(() =>
        {
            craftPanel.Open(recipe);
        });
    }


    public void MakeSp()
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

        owned.Add(Item.ItemType.Spear, 1);

        owned.Save();

        itemsDialog.Rode();

        Debug.Log("‘„‚ðì¬");
    }
}