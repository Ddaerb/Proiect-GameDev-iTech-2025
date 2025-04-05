using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] GameObject[] _characters;
    [SerializeField] GameObject[] _hats;
    [SerializeField] Button _nextCharButton;
    [SerializeField] Button _prevCharButton;
    [SerializeField] Button _nextHatButton;
    [SerializeField] Button _prevHatButton;
    [SerializeField] Button _createCharButton;
    [SerializeField] TextMeshProUGUI _characterNameText;
    [SerializeField] TextMeshProUGUI _hatNameText;

    private Vector3[] _hatSizes;
    private GameObject _selectedCharacter;
    private GameObject _selectedHat;
    private GameObject _instantiatedCharacter;
    private GameObject _instantiatedHat;
    private int _selectedCharacterIndex = 0;
    private int _selectedHatIndex = 0;

    public GameObject InstantiatedCharacter
    {
        get => _instantiatedCharacter;
    }


    private void Awake()
    {
        // Add button events
        _nextCharButton.onClick.AddListener(NextCharacterSelect);
        _prevCharButton.onClick.AddListener(PrevCharacterSelect);
        _nextHatButton.onClick.AddListener(NextHatSelect);
        _prevHatButton.onClick.AddListener(PrevHatSelect);
        _createCharButton.onClick.AddListener(CreateCharacter);


        // Update the character name text to the first character in the array
        _characterNameText.text = _characters[_selectedCharacterIndex].name;

        // Set the first character from the characters array as selected
        _selectedCharacter = _characters[_selectedCharacterIndex];
        _instantiatedCharacter = Instantiate(_selectedCharacter, transform);

        // Set the hat sizes
        SetHatSizes();
    }

    private void SetHatSizes()
    {
        _hatSizes = new Vector3[_hats.Length];
        // Set the hat sizes for each hat
        _hatSizes[0] = Vector3.zero;
        _hatSizes[1] = new Vector3(0.02f, 0.02f, 0.02f);
        _hatSizes[2] = new Vector3(0.01f, 0.01f, 0.01f);
        _hatSizes[3] = new Vector3(0.03f, 0.03f, 0.03f);
    }

    private void PrevHatSelect()
    {
        Transform hatTransform = FindChildWithTag();
        if (hatTransform != null)
        {
            Destroy(_instantiatedHat);
            if(_selectedHatIndex == 0)
            {
                _selectedHatIndex = _hats.Length - 1;
            }
            else
            {
                _selectedHatIndex--;
            }
            UpdateHat(hatTransform);
        }
        else
        {
            Debug.LogWarning("No HatTransform found for: " + _selectedCharacter.name);
        }
    }

    private void NextHatSelect()
    {
        Transform hatTransform = FindChildWithTag();
        if (hatTransform != null)
        {
            Destroy(_instantiatedHat);
            _selectedHatIndex = (_selectedHatIndex + 1) % _hats.Length;
            UpdateHat(hatTransform);
        }
        else
        {
            Debug.LogWarning("No HatTransform found for: " + _selectedCharacter.name);
        }
    }

    private void PrevCharacterSelect()
    {
        Destroy(_instantiatedCharacter);
        if (_selectedCharacterIndex == 0)
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
        Transform hatTransform = FindChildWithTag();
        UpdateHat(hatTransform);
    }

    private void UpdateCharacter()
    {
        _characterNameText.text = _characters[_selectedCharacterIndex].name;
        _selectedCharacter = _characters[_selectedCharacterIndex];
        _instantiatedCharacter = Instantiate(_selectedCharacter, transform);
    }

    private void UpdateHat(Transform hatTransform)
    {
        _hatNameText.text = _hats[_selectedHatIndex].name;
        _selectedHat = _hats[_selectedHatIndex];
        //_selectedHat.transform.localScale = _hatSizes[_selectedHatIndex];
        _instantiatedHat = Instantiate(_selectedHat, hatTransform);
    }

    private Transform FindChildWithTag()
    {
        Transform[] children = _instantiatedCharacter.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("HatTransform"))
            {
                return child;
            }
        }
        return null;
    }

    private void CreateCharacter()
    {
        // Save the created character in the gamemanager singleton class
        // Transform hatTransform = FindFirstObjectByType<FindHatTransform>().transform;
        GameManager.Instance.SaveCharacter(_selectedCharacter, _selectedHat);
        SceneManager.LoadScene("Level1");
    }
}
