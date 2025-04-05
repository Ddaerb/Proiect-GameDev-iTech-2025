using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton, settingsButton, backButton, quitButton;
    [SerializeField] private GameObject SettingsSubmenu;
    [SerializeField] private GameObject TitleCard, MainMenuObject, MainMenuDisplayData, LoadingText;
    [SerializeField] private Text splashText;

    private byte state = 0; // 0 for title card, 1 for main menu
    private float loadingTimer = -1f; // set to 1 upon loading game
    private List<String> splashTextList = new List<String>();

    // button functions
    public void LoadGame() 
    {
        MainMenuDisplayData.SetActive(false);
        LoadingText.SetActive(true);
        loadingTimer = 1f; // starts loading timer
    }
    
    public void OpenSettings() 
    {
        SettingsSubmenu.SetActive(true);
        startButton.interactable = false;
        settingsButton.interactable = false;
        quitButton.interactable = false;
    }

    public void CloseSettings() 
    {
        SettingsSubmenu.SetActive(false);
        startButton.interactable = true;
        settingsButton.interactable = true;
        quitButton.interactable = true;
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    void Start() 
    {
        splashTextList.Add("This may or may not be a splash text");
        splashTextList.Add("Now with mycelarians!");
        splashTextList.Add("Error 404: ship not found");
        splashText.text = splashTextList[(int)UnityEngine.Random.Range(0, splashTextList.Count)];
    }

    void Update() 
    {
        if(Input.anyKeyDown && state == 0) 
        {
            TitleCard.SetActive(false);
            MainMenuObject.SetActive(true);
            state = 1;
        }
        if(loadingTimer > 0f) 
        {
            loadingTimer -= Time.deltaTime;
        }
        else if(loadingTimer != -1f) // loading timer has finished
        {
            SceneManager.LoadScene("CharacterCreator");
        }
    }
}
