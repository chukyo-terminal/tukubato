using ChukyoTerminal.ArcadeNetworking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private ItemsDialog itemsDialog;
    [SerializeField] private RecipeDialog recipeDialog;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject resumeButtonO;
    [SerializeField] private Button titleButton;

    [SerializeField] private Button itemsButton;
    [SerializeField] private GameObject itemButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject closePanel;

    [SerializeField] private Button makeThrowAxeButton;

    [SerializeField] private Button throwAxeButton;
    [SerializeField] private MakeThrowAxe makethrowAxe;

    [SerializeField] private PlayerInput playerInput;

    public InputActionReference openItemsDialog;
    public InputActionReference throwAxe;
    public InputActionReference pause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        pausePanel.SetActive(false);
        closePanel.SetActive(false);

        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        titleButton.onClick.AddListener(TitleBack);
        itemsButton.onClick.AddListener(OpenItemsDialog);
        closeButton.onClick.AddListener(CloseDialog);
        throwAxeButton.onClick.AddListener(TAxe);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButtonO);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        
    }

    private async void TitleBack()
    {
        Time.timeScale = 1;
        OwnedItemsData.Reset();
        
        await ArcadeNetwork.EndPlayAsync();
        

        SceneManager.LoadScene("TitleScene");
    }

    private void OpenItemsDialog()
    {
        Time.timeScale = 0;
        itemsDialog.Open();
        recipeDialog.Open();
        EventSystem.current.SetSelectedGameObject(itemButton);
        closePanel.SetActive(true);
    }

    private void CloseDialog()
    {
        Time.timeScale = 1;
        itemsDialog.Close();
        recipeDialog.Close();
        closePanel.SetActive(false);

        //  ï¬Ç∂ÇΩèuä‘Ç… Player Ç…ñﬂÇ∑
        
    }

   
    private void TAxe()
    {
        makethrowAxe.ThrowAxe();
        
    }


    void OnEnable()
    {
        openItemsDialog.action.performed += OnOpenInventory;
        throwAxe.action.performed += OnThrowAxe;
        pause.action.performed += OnPause;
    }

    void OnDisable()
    {
        openItemsDialog.action.performed -= OnOpenInventory;
        throwAxe.action.performed -= OnThrowAxe;
        pause.action.performed -= OnPause;
    }

    private void OnOpenInventory(InputAction.CallbackContext ctx)
    {
        OpenItemsDialog();
    }

    private void OnThrowAxe(InputAction.CallbackContext ctx)
    {
        TAxe();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        Pause();
    }
}
