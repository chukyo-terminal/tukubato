using ChukyoTerminal.ArcadeNetworking;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override async void OnDie()
    {
        base.OnDie();
        
        var result = await ArcadeNetwork.EndPlayAsync();
        Debug.Log($"EndPlayAsync: {result.Succeeded}, {result.Message}");
        Debug.Log($"Flags: {ArcadeNetwork.Status.Flags}");
        Debug.Log($"IsInPlay: {ArcadeNetwork.Status.IsInPlay}");

        StartCoroutine(GoToGameOverCoroutine());
    }

    public void Heal(int amount)
    {
        _life += amount;

        // 最大値を超えないようにする
        _life = Mathf.Min(_life, lifeMax);

        // HPバーなどのUI更新が必要ならここで呼ぶ
    
    }

    private IEnumerator GoToGameOverCoroutine()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOverScene");
    }
}
