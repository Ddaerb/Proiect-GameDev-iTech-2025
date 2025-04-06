using UnityEngine;
using UnityEngine.UI;
public class ItemCounterUIScript : MonoBehaviour
{
    [SerializeField] private Texture _forcedTexture;
    private RawImage _rawImage;

    void OnEnable()
    {
        _rawImage = GetComponent<RawImage>();
        ApplyTexture();
    }

    void Update()
    {
        // Continuously reapply texture (for stubborn cases)
        if (_rawImage.texture != _forcedTexture)
        {
            ApplyTexture();
        }
    }

    private void ApplyTexture()
    {
        if (_forcedTexture != null && _rawImage != null)
        {
            _rawImage.texture = _forcedTexture;
            _rawImage.color = Color.white; // Ensure no tint is hiding it
            Debug.Log($"FORCED TEXTURE: {_forcedTexture.name}", this);
        }
    }
}
