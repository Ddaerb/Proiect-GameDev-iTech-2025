using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    private Dictionary<CollectibleType, int> _collectibleScores = new Dictionary<CollectibleType, int>();

    void Start()
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
        // Initialize scores for all collectible types
        foreach (CollectibleType type in System.Enum.GetValues(typeof(CollectibleType)))
        {
            _collectibleScores[type] = 0;
        }
    }

    public void AddScore(int value, CollectibleType type)
    {
        if (_collectibleScores.ContainsKey(type))
        {
            _collectibleScores[type] += value;
            Debug.Log($"Collected {type}: {_collectibleScores[type]}");
            UIManager.Instance.UpdateText(_collectibleScores[type], type);
        }
        else
        {
            Debug.LogWarning($"Collectible type {type} not recognized.");
        }
    }
}
