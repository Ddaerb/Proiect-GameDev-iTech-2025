using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Texture[] _images;
    [SerializeField] private int[] _itemCounts;
    [SerializeField] private RawImage _itemImage;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private GameObject _selectionOptionIndicator;
    [SerializeField] private GameObject _optionUI;
    [SerializeField] private List<RectTransform> _options;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private List<string> dialogues = new List<string>();
    private int _currentOptionIndex = 0;
    private float _talkSpeed = 0.05f;
    private int _itemIndex;

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
        _itemIndex = 0;
        ChangeIndex(0); // initialising item
    }

    private void ChangeIndex(int offset)
    {
        if (_itemIndex + offset < 0) { _itemIndex = _images.Length - 1; }
        else if (_itemIndex + offset > _images.Length - 1) { _itemIndex = 0; }
        else { _itemIndex += offset; }

        _itemImage.texture = _images[_itemIndex];
        _quantityText.text = _itemCounts[_itemIndex].ToString();
    }

    public void GoLeft() { ChangeIndex(-1); }
    public void GoRight() { ChangeIndex(1); }

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
                // Implement gift logic here
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
        switch (type)
        {
            case CollectibleType.Crate:
                _itemCounts[0] = value;
                if (_itemIndex == 0) { _quantityText.text = _itemCounts[_itemIndex].ToString(); }
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
