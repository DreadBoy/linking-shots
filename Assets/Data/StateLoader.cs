using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateLoader : MonoBehaviour
{

    public GameState gameState;

    private void Start()
    {
        DontDestroyOnLoad(this);
        IEnumerable<StateLoader> otherLoaders = FindObjectsOfType<StateLoader>().Where(loader => loader != this);
        if (otherLoaders.Count() > 0)
            Destroy(gameObject);
        if (otherLoaders.Count() == 0 && gameState)
            LoadState(gameState);
    }

    public void LoadState(GameState state)
    {
        gameState = state;
        SceneManager.LoadScene(state.LoadedScene);
    }
}
