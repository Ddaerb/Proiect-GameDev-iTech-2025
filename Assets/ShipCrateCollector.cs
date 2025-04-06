using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShipCrateCollector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int requiredCrates = 3;
    [SerializeField] private float interactionRadius = 5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string endingSceneName = "EndingCutScene";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshPro crateCountText;

    private int depositedCrates = 0;
    private bool playerInRange = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (interactText) interactText.gameObject.SetActive(false);
        UpdateCrateText();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRadius;

        if (playerInRange != wasInRange)
        {
            if (interactText) interactText.gameObject.SetActive(playerInRange);
        }

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TryDepositCrates();
        }
    }

    private void TryDepositCrates()
    {
        int playerCrates = CollectibleManager.Instance.GetCollectibleCount(CollectibleType.Crate);

        if (playerCrates > 0 && depositedCrates < requiredCrates)
        {
            int cratesToDeposit = Mathf.Min(
                playerCrates,
                requiredCrates - depositedCrates
            );

            depositedCrates += cratesToDeposit;
            CollectibleManager.Instance.RemoveCrates(cratesToDeposit);

            UpdateCrateText();

            // Check if all crates have been deposited
            if (depositedCrates >= requiredCrates)
            {
                LoadEndingScene();
            }
        }
    }

    private void LoadEndingScene()
    {
        if (!string.IsNullOrEmpty(endingSceneName))
        {
            SceneManager.LoadScene(endingSceneName);
        }
        else
        {
            Debug.LogError("Ending scene name not set in ShipCrateCollector!");
        }
    }

    private void UpdateCrateText()
    {
        if (crateCountText)
            crateCountText.text = $"{depositedCrates}/{requiredCrates}";
    }

}