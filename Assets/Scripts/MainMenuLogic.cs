using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuLogic : MonoBehaviour
{
    private UIDocument UIDoc;
    [SerializeField] private VisualTreeAsset titleTree, mainTree;

    private Button playGameButton, settingsButton, quitButton;

    void Awake() 
    {
        UIDoc = GetComponent<UIDocument>();
        UIDoc.rootVisualElement = titleTree;
        UIDoc.RegisterCallback<KeyDownEvent>(OnKeyDown);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    // switches to main menu on key down and unregisters key down event
    private void OnKeyDown(KeyDownEvent ev) 
    {
        UIDoc.rootVisualElement = mainTree;

        playGameButton = UIDoc.rootVisualElement.Q("PlayGame");
        settingsButton = UIDoc.rootVisualElement.Q("Settings");
        quitButton = UIDoc.rootVisualElement.Q("QuitGame");

        // registering actions to the main buttons
        playGameButton.RegisterCallback<MouseUpEvent>((evt) => LoadGame);
        settingsButton.RegisterCallback<MouseUpEvent>((evt) => OpenSettings);
        quitButton.RegisterCallback<MouseUpEvent>((evt) => QuitGame);

        UIDoc.UnregisterCallback<KeyDownEvent>(OnKeyDown);
    }
}
