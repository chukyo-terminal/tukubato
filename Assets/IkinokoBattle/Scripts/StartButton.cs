using ChukyoTerminal.ArcadeNetworking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        var button = GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(startButton);
        button.onClick.AddListener(async() =>
        {
            PlayStartResult result = await ArcadeNetwork.TryStartPlayAsync();

            if (result.Succeeded)
            {
                OwnedItemsData.Reset();

                Debug.Log($"StartPlayAsync: {result.Succeeded}, {result.Message}");
                Debug.Log($"Flags: {ArcadeNetwork.Status.Flags}");
                Debug.Log($"IsInPlay: {ArcadeNetwork.Status.IsInPlay}");

                SceneManager.LoadScene("MainScene");
                Time.timeScale = 1;
            }
            else
            {
                Debug.Log($"ƒvƒŒƒC‚ðŽn‚ß‚ç‚ê‚Ü‚¹‚ñ: {result.Message}");
            }

        });
           
        
       
        
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
    }

}
