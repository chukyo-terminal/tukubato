using TMPro;
using UnityEngine;

public class PlayerHPText : MonoBehaviour
{
    [SerializeField] private PlayerStatus player;
    [SerializeField] private TMP_Text hpText;

    void Update()
    {
        hpText.text = $"HP:{player.Life:0}/{player.LifeMax:0}";
    }
}