using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private RawImage _pizzaImage;
    [SerializeField] private TextMeshProUGUI _pizzaQuantityText;

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
    }

    public void UpdateText(int value, CollectibleType type)
    {
        switch(type)
        {
            case CollectibleType.Pizza:
                _pizzaQuantityText.text = value.ToString();
                break;
            default:
                Debug.LogWarning($"No UI element for collectible type: {type}");
                break;
        }
    }
}
