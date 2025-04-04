using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }

    private int _pizzas = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }
    
    public void AddScore(int value)
    {
        _pizzas += value;
        Debug.Log("Score: " + _pizzas);
    }
}
