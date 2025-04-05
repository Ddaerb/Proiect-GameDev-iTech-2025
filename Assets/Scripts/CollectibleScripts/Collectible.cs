using UnityEngine;
using System.Collections.Generic;

public enum CollectibleType
{
    Pizza,
    Icecream
}

public class Collectible : MonoBehaviour
{ 
    [SerializeField] private CollectibleType _collectibleType;
    [SerializeField] private int _value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleManager.Instance.AddScore(_value, _collectibleType);
            // Destroy the collectible item
            Destroy(gameObject);
        }
    }
}
