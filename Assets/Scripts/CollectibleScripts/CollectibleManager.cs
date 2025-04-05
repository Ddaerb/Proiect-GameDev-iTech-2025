using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    private Dictionary<CollectibleType, int> _collectibles = new Dictionary<CollectibleType, int>();

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
    }

    public void AddScore(int value, CollectibleType type)
    {
        // Increment the value for the specified collectible type if it exists.
        _collectibles[type] = _collectibles.ContainsKey(type) ? _collectibles[type] + value : value;
        UIManager.Instance.UpdateText(_collectibles[type], type);
    }
}
