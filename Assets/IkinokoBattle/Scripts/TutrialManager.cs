using ChukyoTerminal.ArcadeNetworking;
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

    private string[] messages =
    {
        "遊んでくれてありがとう！(B/Enterで次へ)",
        "まずは左スティックで移動してみよう！",
        "次は敵をXボタンで攻撃して倒そう！",
        "アイテムをドロップしたよ！拾ってね！(B/Enterで次へ)",
        "Yボタンでアイテム画面を開いて、左スティックで「剣を作る」を選んで剣を作ってみよう！（Bボタンで決定）",
        "次は剣を装備してみよう！（左スティックで剣を選んでBボタンで決定）",
        "武器を装備すると攻撃力がアップする！(B/Enterで次へ)",
        "剣をもう一つ作ってみよう！",
        "武器を追加で作ると武器のレベルが上がってさらに強くなる！(B/Enterで次へ)",
        "次は薬を作ってみよう！",
        "できた薬を使うと体力を回復できる！(B/Enterで次へ)",
        "武器やアイテムをたくさん作って敵を倒し続けよう！(B/Enterで次へ)",
        "敵を倒してポイントがたまったらゲーム終了！最速クリアを目指せ！"
    };

    private int step = 0;

    private void Start()
    {
        Time.timeScale = 1;
        ShowStep(step);

        nextTutorialAction.action.performed += OnNextTutorialInput;

        PlayerEvents.OnMove += OnMove;
        PlayerEvents.OnEnemyKilled += OnKill;
        PlayerEvents.OnCraft += OnCraft;
        PlayerEvents.OnUseItem += OnUse;
        PlayerEvents.OnCraftM += OnCraftM;
    }

    private void OnDestroy()
    {
        if (nextTutorialAction != null)
        {
            nextTutorialAction.action.performed -= OnNextTutorialInput;
        }

        PlayerEvents.OnMove -= OnMove;
        PlayerEvents.OnEnemyKilled -= OnKill;
        PlayerEvents.OnCraft -= OnCraft;
        PlayerEvents.OnUseItem -= OnUse;
        PlayerEvents.OnCraftM -= OnCraftM;
    }

    private void OnNextTutorialInput(InputAction.CallbackContext ctx)
    {
        OnFirstMessageInput();
    }

    private void OnMove()
    {
        CheckStep("move");
    }

    private void OnKill()
    {
        CheckStep("kill");
    }

    private void OnCraft()
    {
        CheckStep("craft");
    }

    private void OnUse()
    {
        CheckStep("use");
    }

    private void OnCraftM()
    {
        CheckStep("craftm");
    }

    private void OnFirstMessageInput()
    {
        if (step == 0 ||
            step == 3 ||
            step == 6 ||
            step == 8 ||
            step == 10 ||
            step == 11)
        {
            NextStep();
        }
    }

    private void CheckStep(string action)
    {
        switch (step)
        {
            case 1:
                if (action == "move") NextStep();
                break;

            case 2:
                if (action == "kill") NextStep();
                break;

            case 4:
                if (action == "craft") NextStep();
                break;

            case 5:
                if (action == "use") NextStep();
                break;

            case 7:
                if (action == "craft") NextStep();
                break;

            case 9:
                if (action == "craftm") NextStep();
                break;
        }
    }

    public void NextStep()
    {
        step++;

        if (step == 2)
        {
            if (spawnPoint == null)
            {
                Debug.LogError("SpawnPoint is missing!");
                return;
            }

            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation);
        }

        if (step < messages.Length)
        {
            ShowStep(step);
        }
    }

    private async void ShowStep(int s)
    {
        StartCoroutine(ChangeText(messages[s]));

        if (s == 12)
        {
            var result = await ArcadeNetwork.EndPlayAsync();

            // await中に破棄された場合
            if (this == null)
            {
                return;
            }

            Debug.Log($"EndPlayAsync: {result.Succeeded}, {result.Message}");
            Debug.Log($"Flags: {ArcadeNetwork.Status.Flags}");
            Debug.Log($"IsInPlay: {ArcadeNetwork.Status.IsInPlay}");

            StartCoroutine(ReturnToTitleAfterDelay());
        }
    }

    private IEnumerator ChangeText(string newText)
    {
        tutorialText.CrossFadeAlpha(0, 0.3f, false);

        yield return new WaitForSecondsRealtime(0.3f);

        tutorialText.text = newText;

        tutorialText.CrossFadeAlpha(1, 0.3f, false);
    }

    public IEnumerator ReturnToTitleAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
