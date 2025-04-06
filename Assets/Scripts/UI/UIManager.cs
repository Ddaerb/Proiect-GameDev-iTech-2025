using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private RawImage _pizzaImage;
    [SerializeField] private TextMeshProUGUI _pizzaQuantityText;
    [SerializeField] private GameObject _selectionOptionIndicator;
    [SerializeField] private GameObject _optionUI;
    [SerializeField] private List<RectTransform> _options;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private List<string> dialogues = new List<string>();
    private int _currentOptionIndex = 0;
    private float _talkSpeed = 0.05f;

    private enum Options
    {
        Close,
        Attack,
        Gift,
        Talk
    }

    public bool OptionUIActive => _optionUI.activeSelf;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes.
            PopulateDialogueLines();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance.
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void PopulateDialogueLines()
    {
        dialogues.Add("You want peace? I want parts. Maybe we both get lucky.");
        dialogues.Add("Peace is like compost—it stinks at first, but it grows on you.");
        dialogues.Add("Do not touch the glowing mushrooms. Not unless you want to relive your most awkward memories… in reverse.");
        dialogues.Add("You smell like someone who’s never eaten engine grease. Fancy.");
        dialogues.Add("Peace is hard. War is easy. Snacks are preferred.");
    }

    private void HandleInput()
    {
        if(_optionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveSelectionDown();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveSelectionUp();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteOption();
            }
        }
    }

    private void MoveSelectionDown()
    {
        _currentOptionIndex = (_currentOptionIndex + 1) % _options.Count;
        UpdateSelectPosition();
    }

    private void MoveSelectionUp()
    {
        _currentOptionIndex = (_currentOptionIndex - 1 + _options.Count) % _options.Count;
        UpdateSelectPosition();
    }

    public void UpdateSelectPosition()
    {
        if (_options.Count > 0)
        {
            Vector3 newPosition = _selectionOptionIndicator.transform.position;
            newPosition.y = _options[_currentOptionIndex].position.y;
            _selectionOptionIndicator.transform.position = newPosition;
        }
    }

    private void ExecuteOption()
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction == null)
        {
            Debug.LogWarning("PlayerInteraction script not found.");
            return;
        }

        switch ((Options)_currentOptionIndex)
        {
            case Options.Close:
                ToggleOptionMenu();
                break;
            case Options.Attack:
                // Implement attack logic here
                break;
            case Options.Gift:
                ToggleDialogue();
                ToggleOptionMenu();
                Gift();
                break;
            case Options.Talk:
                ToggleDialogue();
                ToggleOptionMenu();
                ShowDialogue(dialogues[Random.Range(0, dialogues.Count)]);
                break;
            default:
                Debug.LogWarning($"No action defined for option: {_currentOptionIndex}");
                break;
        }
    }
    public void UpdateText(int value, CollectibleType type)
    {
        switch(type)
        {
            case CollectibleType.Pizza:
                _pizzaQuantityText.text = value.ToString();
                break;
            default:
                Debug.LogWarning($"No UI element for collectible type: {type}");
                break;
        }
    }

    public void ToggleOptionMenu()
    {
        _optionUI.SetActive(!_optionUI.activeSelf);
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.TogglePlayerMovement();
        }
    }

    private void ToggleDialogue()
    {
        _dialogueUI.SetActive(!_dialogueUI.activeSelf);
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.TogglePlayerMovement();
        }
    }

    public void Gift()
    {
        StartCoroutine(GiftCoroutine());
    }

    private IEnumerator GiftCoroutine()
    {
        _dialogueText.text = "";
        _dialogueText.gameObject.SetActive(true);
        string dialogue = string.Empty;
        CollectibleType? giftedType = CollectibleManager.Instance.RemoveCollectible();
        if (giftedType.HasValue)
        {
            dialogue = $"You have given me a {giftedType}. I will help you escape to freedom by giving you spaceship parts to repair your spaceship.";
        }
        else
        {
            dialogue = "You have nothing to gift.";
        }

        foreach (char letter in dialogue)
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_talkSpeed);
            }
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the dialogue
        ToggleDialogue();
    }


    public void ShowDialogue(string dialogueLine)
    {
        StartCoroutine(DisplayDialogue(dialogueLine));
    }

    private IEnumerator DisplayDialogue(string dialogueLine)
    {
        _dialogueText.text = "";
        _dialogueText.gameObject.SetActive(true);
        foreach (char letter in dialogueLine.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_talkSpeed); // Adjust the duration as needed
        }
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the dialogue
        ToggleDialogue();
    }
}
