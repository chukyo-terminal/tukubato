using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutrialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private InputActionReference nextTutorialAction;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    private string[] messages = {
        "遊んでくれてありがとう！(A/Enterで次へ)",
        "まずは移動してみよう！",
        "次は敵を攻撃して倒そう！",
        "アイテムをドロップしたよ！(A/Enterで次へ)",
        "アイテムで剣を作ってみよう！",
        "次は剣を装備してみよう！",
        "武器を装備すると攻撃力や攻撃スピードが変化する！(A/Enterで次へ)",
        "さらに、武器を追加で作ると武器のレベルが上がって強くなる！(A/Enterで次へ)",
        "武器やアイテムをたくさん作って敵を倒し続けよう！(A/Enterで次へ)",
        "敵を倒してポイントがたまったらゲーム終了！最速クリアを目指せ！"
    };

    private int step = 0;

    void Start()
    {
        ShowStep(step);
        nextTutorialAction.action.performed += ctx => OnFirstMessageInput();
        PlayerEvents.OnMove += () => CheckStep("move");
        PlayerEvents.OnEnemyKilled += () => CheckStep("kill");
        PlayerEvents.OnCraft += () => CheckStep("craft");
        PlayerEvents.OnUseItem += () => CheckStep("use");
    }

    private void OnFirstMessageInput()
    {
        if (step == 0)
        {
            NextStep();
        }

        if (step == 3)
        {
            NextStep();
        }

        if (step == 6)
        {
            NextStep();
        }

        if (step == 7)
        {
            NextStep();
        }

        if (step == 8)
        {
            NextStep();
        }
    }


    private void CheckStep(string action)
    {
        switch (step)
        {
            case 1: // 移動フェーズ
                if (action == "move") NextStep();
                break;

           

            case 2: // 敵撃破フェーズ
                if (action == "kill") NextStep();
                break;

            

            case 4: // アイテム作成フェーズ
                if (action == "craft") NextStep();
                break;

            case 5: // アイテム使用フェーズ
                if (action == "use") NextStep();
                break;


        }
    }

    public void NextStep()
    {
        step++;

        if(step == 2 )
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        if (step < messages.Length)
        {
            ShowStep(step);
        }
    }

    private void ShowStep(int s)
    {
        StartCoroutine(ChangeText(messages[s]));
        if (s == 9)
        {
            StartCoroutine(ReturnToTitleAfterDelay());
        }
    }

    private IEnumerator ChangeText(string newText)
    {
        // フェードアウト
        tutorialText.CrossFadeAlpha(0, 0.3f, false);
        yield return new WaitForSecondsRealtime(0.3f);

        // テキスト差し替え
        tutorialText.text = newText;

        // フェードイン
        tutorialText.CrossFadeAlpha(1, 0.3f, false);
    }

    public IEnumerator ReturnToTitleAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f); // ← timeScale=0でも動く

        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

}
