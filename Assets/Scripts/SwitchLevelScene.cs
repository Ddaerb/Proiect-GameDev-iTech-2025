using UnityEngine;

public class SwitchLevelScene : MonoBehaviour
{
    // This script is responsible for switching between different levels in the game.
    enum Levels
    {
        FirstLevel,
        MapEast,
        MapNorth
    }

    [SerializeField] private Levels _level;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SwitchLevel();
        }
    }

    private void SwitchLevel()
    {
        switch (_level)
        {
            case Levels.MapEast:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MapEast");
                break;
            case Levels.MapNorth:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MapNorth");
                break;
            case Levels.FirstLevel:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
                break;
            default:
                Debug.LogError("Invalid level selected.");
                break;
        }
    }
}
