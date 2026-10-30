using System;
using ChukyoTerminal.ArcadeNetworking;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    // START できる条件: プレイ受付中で、クレジットがあり、プレイ中でない
    private static readonly ArcadeCondition CanStart = new ArcadeCondition()
        .Require(ArcadeFlags.Accepting | ArcadeFlags.HasCredit)
        .Forbid(ArcadeFlags.Playing);

   
    [SerializeField] private GameObject pressStart;
    [SerializeField] private Text creditText;

    private IDisposable _watch;

    private void OnEnable()
    {
        // 条件の結果が変わるたびに呼ばれる。登録したときにも 1 回呼ばれる
        _watch = ArcadeNetwork.Observe(CanStart, canStart =>
        {
            pressStart.SetActive(canStart);
           
        });

        ArcadeNetwork.CreditsChanged += ShowCredits;
        ShowCredits(ArcadeNetwork.Status.AvailableCredits);
    }

    private void OnDisable()
    {
        // 登録したものは必ず解除する
        _watch.Dispose();
        ArcadeNetwork.CreditsChanged -= ShowCredits;
    }

    private void ShowCredits(int credits)
    {
        creditText.text = $"残りプレイ数 {credits}";
    }
}