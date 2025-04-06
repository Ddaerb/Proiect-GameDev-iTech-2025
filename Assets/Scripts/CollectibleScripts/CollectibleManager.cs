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
            SoundManager.Instance.PlaySFX(SoundManager.Instance.collectibleSound);
            UIManager.Instance.UpdateText(_collectibleScores[type], type);
        }
        else
        {
            Debug.LogWarning($"Collectible type {type} not recognized.");
        }
    }

    public int GetCollectibleCount(CollectibleType type)
    {
        if (_collectibleScores.ContainsKey(type))
        {
            return _collectibleScores[type];
        }
        return 0;
    }

    public CollectibleType? GetRandomNonCrateCollectible()
    {
        List<CollectibleType> nonCrateCollectibles = new List<CollectibleType>();

        foreach (var kvp in _collectibleScores)
        {
            if (kvp.Key != CollectibleType.Crate && kvp.Value > 0)
            {
                nonCrateCollectibles.Add(kvp.Key);
            }
        }

        if (nonCrateCollectibles.Count > 0)
        {
            CollectibleType selectedType = nonCrateCollectibles[Random.Range(0, nonCrateCollectibles.Count)];
            _collectibleScores[selectedType]--;
            UIManager.Instance.UpdateText(_collectibleScores[selectedType], selectedType);
            return selectedType;
        }

        return null;
    }

    public void RemoveCrates(int amount)
    {
        if (_collectibleScores.ContainsKey(CollectibleType.Crate))
        {
            _collectibleScores[CollectibleType.Crate] = Mathf.Max(
                0,
                _collectibleScores[CollectibleType.Crate] - amount
            );

            UIManager.Instance.UpdateText(
                _collectibleScores[CollectibleType.Crate],
                CollectibleType.Crate
            );
        }
    }
}
