using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton, settingsButton, backButton, quitButton;
    [SerializeField] private GameObject SettingsSubmenu;
    [SerializeField] private GameObject TitleCard, MainMenuObject, MainMenuDisplayData, LoadingText;
    [SerializeField] private Text splashText;

    [Header("Scene Transition")]
    [SerializeField] private float loadingDelay = 1f;
    [SerializeField] private string cutsceneName = "IntroCutScene";
    [SerializeField] private string level1Name = "Level1";

    private byte state = 0; // 0 for title card, 1 for main menu
    private float loadingTimer = -1f;
    private List<String> splashTextList = new List<String>();

    void Start()
    {
        // Initialize splash texts
        splashTextList.Add("This may or may not be a splash text");
        splashTextList.Add("Now with mycelarians!");
        splashTextList.Add("Error 404: ship not found");
        splashText.text = splashTextList[(int)UnityEngine.Random.Range(0, splashTextList.Count)];

        // Setup button listeners
        startButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(OpenSettings);
        backButton.onClick.AddListener(CloseSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void LoadGame()
    {
        MainMenuDisplayData.SetActive(false);
        LoadingText.SetActive(true);
        loadingTimer = loadingDelay; // Start loading countdown
    }

    public void OpenSettings()
    {
        SettingsSubmenu.SetActive(true);
        SetMainMenuButtonsInteractable(false);
    }

    public void CloseSettings()
    {
        SettingsSubmenu.SetActive(false);
        SetMainMenuButtonsInteractable(true);
    }

    private void SetMainMenuButtonsInteractable(bool interactable)
    {
        startButton.interactable = interactable;
        settingsButton.interactable = interactable;
        quitButton.interactable = interactable;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.anyKeyDown && state == 0)
        {
            TitleCard.SetActive(false);
            MainMenuObject.SetActive(true);
            state = 1;
        }

        if (loadingTimer > 0f)
        {
            loadingTimer -= Time.deltaTime;
        }
        else if (loadingTimer != -1f)
        {
            SceneManager.LoadScene(cutsceneName);
            loadingTimer = -1f;
        }
    }
}