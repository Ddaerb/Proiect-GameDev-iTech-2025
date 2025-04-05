using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    void Start()
    {
        Instantiate(GameManager.Instance.Player, transform);
        Transform hatTransform = FindFirstObjectByType<FindHatTransform>().transform;

        Instantiate(GameManager.Instance.Hat, hatTransform);
    }
}
