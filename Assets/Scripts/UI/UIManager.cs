using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Texture[] _images;
    [SerializeField] private int[] _itemCounts;
    [SerializeField] private RawImage _itemImage;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private int itemIndex;

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

        itemIndex = 0;
    }

    public void UpdateText(int value, CollectibleType type)
    {
        switch(type)
        {
            case CollectibleType.Pizza:
                _itemCounts[itemIndex] = value;
                _quantityText.text = _itemCounts[itemIndex].ToString();
                break;
            default:
                Debug.LogWarning($"No UI element for collectible type: {type}");
                break;
        }
    }

    private void ChangeIndex(int offset) 
    {
        // dealing with over/underflows
        if (itemIndex + offset < 0) { itemIndex = _images.Length; }
        else if (itemIndex + offset > _images.Length) { itemIndex = 0; }

        // setting up item counter ui
        itemIndex += offset;
        SetItem(itemIndex);
        
    }

    private void SetItem(int index) 
    {
        _itemImage.texture = _images[index];
        _quantityText.text = _itemCounts[index].ToString();
    }
}
