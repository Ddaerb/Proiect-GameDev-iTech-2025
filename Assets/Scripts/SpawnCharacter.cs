using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    void Start()
    {
        // Set the scale of the player to match the map
        GameObject player = Instantiate(GameManager.Instance.Player, transform);
        player.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        Transform hatTransform = FindFirstObjectByType<FindHatTransform>().transform;
        if (GameManager.Instance.Hat != null)
        {
            Instantiate(GameManager.Instance.Hat, hatTransform);
        }
    }
}
