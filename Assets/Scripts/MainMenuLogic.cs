using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class MainMenuLogic : MonoBehaviour
{
    private UIDocument mainMenuDoc;
    private Button startButton, quitButton;

    void Awake() 
    {
        // initialising components
        mainMenuDoc = GetComponent<UIDocument>();
        startButton = mainMenuDoc.rootVisualElement.Q("PlayButton") as Button;
        quitButton = mainMenuDoc.rootVisualElement.Q("QuitButton") as Button;

        startButton.RegisterCallback<ClickEvent>(LoadGame);
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void OnDisable() 
    {
        startButton.UnregisterCallback<ClickEvent>(LoadGame);
        quitButton.UnregisterCallback<ClickEvent>(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function to load game
    private void LoadGame(ClickEvent evt) 
    {
        SceneManager.LoadScene("Level1");
    }

    // function to quit game
    private void QuitGame(ClickEvent evt) 
    {
        Application.Quit();
    }
} 
