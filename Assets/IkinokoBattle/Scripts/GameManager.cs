using TMPro;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private TMP_Text clearTimeText;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private PlayerController playerController;

    public static GameManager instance;

    public int currentPoint = 0;
    public int clearPoint = 10;
    bool isCleared = false;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        clearPanel.SetActive(false);
    }
    
    public void AddPoint(int value)
    {
        if (isCleared) return;

        currentPoint += value;
        Debug.Log("ポイント：" + currentPoint);

        if(currentPoint >= clearPoint)
        {
            OnGameClear();
        }
    }

    public void OnGameClear()
    {
        // タイマー停止
        gameTimer.StopTimer();

        // 経過時間を取得
        float t = gameTimer.ElapsedTime;

        // 分秒に変換
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);

        // 表示
        clearTimeText.text = $"{minutes:00}:{seconds:00}";

        // クリア画面を表示
        clearPanel.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(ReturnToTitleAfterDelay());
    }


    public IEnumerator ReturnToTitleAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f); // ← timeScale=0でも動く

        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
