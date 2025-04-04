using UnityEngine;

public class Collectible : MonoBehaviour
{
    private enum CollectibleType
    {
        Pizza,
        Icecream
    }

    [SerializeField] private CollectibleType _collectibleType;
    [SerializeField] private int _value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(_value);
            // Destroy the collectible item
            Destroy(gameObject);
        }
    }
}
