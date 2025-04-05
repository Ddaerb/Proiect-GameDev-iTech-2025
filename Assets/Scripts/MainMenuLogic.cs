using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;

public class MainMenuLogic : MonoBehaviour
{
    private UIDocument UIDoc;
    [SerializeField] private VisualTreeAsset titleTree, mainTree;
    private VisualElement titleAsset, mainMenuAsset;
    private byte state = 0; // 0 for title card, 1 for main menu

    private Button playGameButton, settingsButton, quitButton;

    // button actions
    Action LoadGame = () => 
    {
        SceneManager.LoadScene("Level1");
    };

    Action OpenSettings = () => 
    {
        Debug.Log("no settings lmfao");
    };

    Action QuitGame = () => 
    {
        Application.Quit();
    };


    void Awake() 
    {
        titleAsset = titleTree.Instantiate();
        mainMenuAsset = mainTree.Instantiate();

        UIDoc = GetComponent<UIDocument>();

        
    }

    void Start() 
    {
        playGameButton = UIDoc.rootVisualElement.Q<Button>("PlayGame");
            settingsButton = UIDoc.rootVisualElement.Q<Button>("Settings");
            quitButton = UIDoc.rootVisualElement.Q<Button>("QuitGame");

            // Ensure buttons are found
            if (playGameButton != null && settingsButton != null && quitButton != null) 
            {
                Debug.Log("suiii");
                // Register actions to buttons
                playGameButton.RegisterCallback<MouseUpEvent>((evt) => LoadGame());
                settingsButton.RegisterCallback<MouseUpEvent>((evt) => OpenSettings());
                quitButton.RegisterCallback<MouseUpEvent>((evt) => QuitGame());
            }
    }


    void Update() 
    {   
        /*
        if(Input.anyKeyDown && state == 0) 
        {
            Debug.Log("bruh");
            // Switch to the main menu
            titleAsset.RemoveFromHierarchy();
            UIDoc.rootVisualElement.Add(mainMenuAsset);

            // Get buttons
            playGameButton = UIDoc.rootVisualElement.Q<Button>("PlayGame");
            settingsButton = UIDoc.rootVisualElement.Q<Button>("Settings");
            quitButton = UIDoc.rootVisualElement.Q<Button>("QuitGame");

            // Ensure buttons are found
            if (playGameButton != null && settingsButton != null && quitButton != null) 
            {
                // Register actions to buttons
                playGameButton.RegisterCallback<MouseUpEvent>((evt) => LoadGame());
                settingsButton.RegisterCallback<MouseUpEvent>((evt) => OpenSettings());
                quitButton.RegisterCallback<MouseUpEvent>((evt) => QuitGame());
            }

            // Change state to avoid re-triggering
            state = 1;
        }
        */
        
    }
}
