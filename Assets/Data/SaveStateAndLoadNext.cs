using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(InteractionTrigger))]
public class SaveStateAndLoadNext : MonoBehaviour
{
    public InteractionTrigger interactionTrigger;
    public Player player;
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public string nextScene;

    private void Start()
    {
        interactionTrigger.text = interactionTrigger.action;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionTrigger.keyCode) && (player.transform.position2D() - boxCollider.bounds.center.ToVector2()).magnitude < interactionTrigger.distance)
        {
            SaveState();
            NextScene();
        }
    }

    public void SaveState()
    {
        GameState state = ScriptableObject.CreateInstance<GameState>();
        state.LoadedScene = nextScene;
        state.Weapon = Instantiate(player.weapon);
        var saveFile = JsonUtility.ToJson(state);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void Reset()
    {
        interactionTrigger = GetComponent<InteractionTrigger>();
        player = FindObjectOfType<Player>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(SaveStateAndLoadNext)), CanEditMultipleObjects]
public class SaveStateAndLoadNextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var saver = target as SaveStateAndLoadNext;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(saver.nextScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Next Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("nextScene");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

