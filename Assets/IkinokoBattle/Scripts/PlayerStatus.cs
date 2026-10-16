using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnDie()
    {
        base.OnDie();
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
