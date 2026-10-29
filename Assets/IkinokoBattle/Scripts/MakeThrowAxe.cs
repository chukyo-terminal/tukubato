using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class MakeThrowAxe : MonoBehaviour
{
    [SerializeField] private Button makeTAButton;
    [SerializeField] private GameObject throwAxePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ItemsDialog itemsDialog;
    [SerializeField] private CraftConfirmPanel craftPanel;
    [SerializeField] private CraftRecipe recipe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        makeTAButton.onClick.AddListener(() =>
        {
            craftPanel.Open(recipe);
        });
    }


    public void MakeTA()
    {
        var owned = OwnedItemsData.Instance;

        const int needWood = 2;
        const int needStone = 2;

        var wood = owned.GetItem(Item.ItemType.Wood);
        var stone = owned.GetItem(Item.ItemType.Stone);

        if(wood == null || wood.Number < needWood || stone == null || stone.Number < needStone)
        {
            Debug.Log("ëfçﬁÇ™ë´ÇËÇ‹ÇπÇÒ");
            return;
        }

        owned.Use(Item.ItemType.Wood, needWood);
        owned.Use(Item.ItemType.Stone, needStone);

        owned.Add(Item.ItemType.ThrowAxe, 1);

        owned.Save();

        itemsDialog.Rode();

        Debug.Log("ìäÇ∞ïÄÇçÏê¨");
    }

    public void ThrowAxe()
    {
        var owned = OwnedItemsData.Instance;

        var axeItem = owned.GetItem(Item.ItemType.ThrowAxe);

        if (axeItem == null || axeItem.Number <= 0)
        {
            Debug.Log("ìäÇ∞ïÄÇ™Ç†ÇËÇ‹ÇπÇÒ");
            return;
        }

        // ìäÇ∞ïÄÇ1Ç¬è¡îÔ
        owned.Use(Item.ItemType.ThrowAxe, 1);
        owned.Save();

        // GameObject Çê∂ê¨ÇµÇƒîÚÇŒÇ∑
        GameObject axe = Instantiate(throwAxePrefab, spawnPoint.position, Quaternion.LookRotation(spawnPoint.right));

        var throwable = axe.GetComponent<ThrowableAxe>();
        if (throwable != null)
        {
            throwable.Throw(spawnPoint.forward);
        }

        Debug.Log("ìäÇ∞ïÄÇìäÇ∞Ç‹ÇµÇΩÅI");
    }

}
