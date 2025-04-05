using UnityEngine;

public class RotateCollectible : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 50f;
    [SerializeField] float _floatingAmplitude = 0.2f;
    [SerializeField] float _floatingFrequency = 1f;
    [SerializeField] float _floatingHeight = 0.5f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        float newY = Mathf.Sin(Time.time * _floatingFrequency) * _floatingAmplitude;
        // Ensure the collectible floats above the ground
        newY = Mathf.Abs(newY) + _floatingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
