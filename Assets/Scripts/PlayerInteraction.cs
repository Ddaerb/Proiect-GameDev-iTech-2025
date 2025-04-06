using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // The radius within which the player can interact with objects
    private float _interactionRadius = 2f;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        // Get the PlayerMovement component attached to the player
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckNearbyNPC();
    }

    private void CheckNearbyNPC()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _interactionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("NPC"))
                {
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.menuSelect);
                    UIManager.Instance.ToggleOptionMenu();
                }
            }

        }
    }

    public void TogglePlayerMovement()
    {
        if (UIManager.Instance.OptionUIActive)
        {
            _playerMovement.enabled = false;
        }
        else
        {
            _playerMovement.enabled = true;
        }
    }
}
