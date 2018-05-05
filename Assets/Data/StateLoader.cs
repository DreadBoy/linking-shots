using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateLoader : MonoBehaviour
{

    public GameState gameState;

    private void Start()
    {
        DontDestroyOnLoad(this);
        StateLoader otherLoader = FindObjectsOfType<StateLoader>().FirstOrDefault(loader => loader != this);
        if (!otherLoader)
            Destroy(gameObject);
        if (otherLoader && gameState)
            LoadState(gameState);
    }

    public void LoadState(GameState state)
    {
        gameState = state;
        SceneManager.LoadScene(state.LoadedScene);
    }
}
