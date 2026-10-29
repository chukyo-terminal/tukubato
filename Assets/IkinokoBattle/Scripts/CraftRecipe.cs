using UnityEngine;

[System.Serializable]
public class CraftRecipe
{
    public Item.ItemType resultItem;   // 作るアイテム
    public int resultCount;            // 作る個数

    public Item.ItemType[] materials;  // 必要素材
    public int[] materialCounts;       // 必要数
}
