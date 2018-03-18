using UnityEngine;
using UnityEngine.UI;

public class MainMenuLoader : MonoBehaviour
{

    public GameObject Entry;
    public StateLoader stateLoader;
    public GameState[] gameSaves;

    void Start()
    {
        foreach (GameState save in gameSaves)
        {
            var obj = Instantiate(Entry);
            obj.transform.Find("Text").GetComponent<Text>().text = save.LoadedScene;
            obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                stateLoader.LoadState(save);
            });
            obj.transform.SetParent(transform, false);
        };
        Destroy(Entry);
    }


}
