using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }
    private GameObject _player;
    private GameObject _hat;
    private Transform _hatTransform;

    public GameObject Player
    {
        get => _player;
    }

    public GameObject Hat
    {
        get => _hat;
    }

    public Transform HatTransform
    {
        get => _hatTransform;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void SaveCharacter(GameObject createdCharacter, GameObject selectedHat)
    {
        _player = createdCharacter;
        _hat = selectedHat;
    }

    public void TogglePlayerMovement()
    {
        if (_player != null)
        {
            PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = !playerMovement.enabled;
            }
        }
    }
}
