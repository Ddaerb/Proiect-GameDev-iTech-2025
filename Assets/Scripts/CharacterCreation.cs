using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] GameObject[] _characters;
    [SerializeField] Button _nextCharButton;
    [SerializeField] Button _prevCharButton;
    [SerializeField] TextMeshProUGUI _characterNameText;

    private GameObject _selectedCharacter;
    private GameObject _instantiatedCharacter;
    private int _selectedCharacterIndex = 0;


    private void Awake()
    {
        // Add button events
        _nextCharButton.onClick.AddListener(NextCharacterSelect);
        _prevCharButton.onClick.AddListener(PrevCharacterSelect);

        // Update the character name text to the first character in the array
        _characterNameText.text = _characters[_selectedCharacterIndex].name;

        // Set the first character from the characters array as selected
        _selectedCharacter = _characters[_selectedCharacterIndex];
        _instantiatedCharacter = Instantiate(_selectedCharacter, transform);
    }

    private void PrevCharacterSelect()
    {
        Destroy(_instantiatedCharacter);
        if(_selectedCharacterIndex == 0)
        {
            _selectedCharacterIndex = _characters.Length - 1;
        }
        else
        {
            _selectedCharacterIndex--;
        }
        UpdateCharacter();
    }

    private void NextCharacterSelect()
    {
        Destroy(_instantiatedCharacter);
        _selectedCharacterIndex = (_selectedCharacterIndex + 1) % _characters.Length;
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        _characterNameText.text = _characters[_selectedCharacterIndex].name;
        _selectedCharacter = _characters[_selectedCharacterIndex];
        _instantiatedCharacter = Instantiate(_selectedCharacter, transform);
    }
}
