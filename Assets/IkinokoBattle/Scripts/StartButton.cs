using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var button = GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(startButton);
        button.onClick.AddListener(() =>
        {
            OwnedItemsData.Reset();
            SceneManager.LoadScene("MainScene");
            Time.timeScale = 1;
        });
        
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
    }

}
