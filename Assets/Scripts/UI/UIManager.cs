using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    private int _currentOptionIndex = 0;
    private int _itemIndex = 0;

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
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance.
        }
        _itemIndex = 0;
        ChangeIndex(0); // initialising item
    }

    private void Update()
    {
        HandleInput();
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
                // Implement talk logic here
                break;
            default:
                Debug.LogWarning($"No action defined for option: {_currentOptionIndex}");
                break;
        }
    }

    public void ChangeIndex(int offset) 
    {
        if (_itemIndex + offset < 0) { _itemIndex = _images.Length - 1; }
        else if (_itemIndex + offset > _images.Length - 1) { _itemIndex = 0; }
        else { _itemIndex += offset; }

        _itemImage.texture = _images[_itemIndex];
        _quantityText.text = _itemCounts[_itemIndex].ToString();
    }

    public void UpdateText(int value, CollectibleType type)
    {
        switch(type)
        {
            case CollectibleType.Pizza:
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
}
