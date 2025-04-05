using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public Button startButton, settingsButton, backButton, quitButton;
    public GameObject SettingsSubmenu;
    public GameObject TitleCard, MainMenuScreen;
    public Text splashText;

    private byte state = 0; // 0 for title card, 1 for main menu
    private List<String> splashTextList = new List<String>();

    // button functions
    public void LoadGame() 
    {
        SceneManager.LoadScene("CharacterCreator");
    }
    
    public void OpenSettings() 
    {
        Debug.Log("cuh");
        SettingsSubmenu.SetActive(true);
        startButton.interactable = false;
        settingsButton.interactable = false;
        quitButton.interactable = false;
    }

    public void CloseSettings() 
    {
        Debug.Log("bruh");
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
            MainMenuScreen.SetActive(true);
            state = 1;
        }
    }
}
