using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftConfirmPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text infoText;
    [SerializeField] private Text consumeText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private ItemsDialog itemsDialog;
    [SerializeField] private GameObject itemButton;

    private CraftRecipe currentRecipe;

    void Start()
    {
        panel.SetActive(false);
        yesButton.onClick.AddListener(OnYes);
        noButton.onClick.AddListener(OnNo);
    }

    public void Open(CraftRecipe recipe)
    {
        currentRecipe = recipe;
        panel.SetActive(true);

        infoText.text = $"{recipe.resultItem} を作りますか？";

        // 消費素材の表示
        consumeText.text = "";
        var owned = OwnedItemsData.Instance;

        for (int i = 0; i < recipe.materials.Length; i++)
        {
            var type = recipe.materials[i];
            var need = recipe.materialCounts[i];
            var have = owned.GetItem(type)?.Number ?? 0;

            consumeText.text += $"消費：{type} × {need}（所持：{have}）\n";
        }

        EventSystem.current.SetSelectedGameObject(yesButton.gameObject);
    }

    private void OnYes()
    {
        var owned = OwnedItemsData.Instance;

        // 素材チェック
        for (int i = 0; i < currentRecipe.materials.Length; i++)
        {
            var type = currentRecipe.materials[i];
            var need = currentRecipe.materialCounts[i];
            var have = owned.GetItem(type)?.Number ?? 0;

            if (have < need)
            {
                Debug.Log("素材が足りません");
                panel.SetActive(false);
                EventSystem.current.SetSelectedGameObject(itemButton);
                return;
            }
        }

        // 素材消費
        for (int i = 0; i < currentRecipe.materials.Length; i++)
        {
            owned.Use(currentRecipe.materials[i], currentRecipe.materialCounts[i]);
        }

        // アイテム追加
        owned.Add(currentRecipe.resultItem, currentRecipe.resultCount);
        owned.Save();

        Debug.Log($"{currentRecipe.resultItem} を作成しました");

        itemsDialog.Rode();

        EventSystem.current.SetSelectedGameObject(itemButton);

        if(currentRecipe.resultItem == Item.ItemType.Sword)
        {
            PlayerEvents.OnCraft?.Invoke();
        }

        if (currentRecipe.resultItem == Item.ItemType.Medicine)
        {
            PlayerEvents.OnCraftM?.Invoke();
        }

        panel.SetActive(false);
    }

    private void OnNo()
    {
        EventSystem.current.SetSelectedGameObject(itemButton);
        panel.SetActive(false);
    }
}

