using System.Collections;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public static NPCInteraction Instance { get; private set; }
    TextMeshProUGUI _textBox;
    [SerializeField] float _textSpeed = 0.4f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void Start()
    {
        gameObject.SetActive(true);
        _textBox = TryGetComponent<TextMeshProUGUI>(out var textBox) ? textBox : null;
    }

    public void Speak()
    {
        GameManager.Instance.TogglePlayerMovement();
        StartCoroutine(TypeLetter());
        GameManager.Instance.TogglePlayerMovement();
    }

    IEnumerator TypeLetter()
    {
        foreach(char letter in "Hello! How can I help you?")
        {
            _textBox.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
