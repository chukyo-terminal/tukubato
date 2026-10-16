using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private float startTime;
    public TMP_Text timerText;
    private bool isRunning = true;

    public float ElapsedTime => Time.time - startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (!isRunning) return;
        float elapsed = Time.time - startTime;
        timerText.text = elapsed.ToString("F2");
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
