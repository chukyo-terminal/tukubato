using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverTextAnimator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var transformCache = transform;
        var defaultPosition = transform.localPosition;
        transformCache.localPosition = new Vector3(0, 300f);
        transformCache.DOLocalMove(defaultPosition, 1f)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            Debug.Log("GAME OVER!!");
            transformCache.DOShakePosition(1.5f, 100);
        });
        DOVirtual.DelayedCall(10, () =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }
}
