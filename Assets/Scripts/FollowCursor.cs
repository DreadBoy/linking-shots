using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FollowCursor : MonoBehaviour
{
    public Player player;
    new public Camera camera;
    public PolygonCollider2D SceneBounds;

    private void Reset()
    {
        player = FindObjectOfType<Player>();
        camera = Camera.main;
    }

    void Update()
    {
        var playerPos = camera.WorldToScreenPoint(player.transform.position);
        playerPos.z = 0;
        var deltaScreen = Input.mousePosition - playerPos;
        var deltaWorld = camera.ScreenToWorldPoint(deltaScreen) - camera.ScreenToWorldPoint(new Vector3(0, 0));
        var newPosition = player.transform.position + deltaWorld;
        if (Physics2D.OverlapPoint(newPosition) == SceneBounds)
            transform.position = newPosition;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(FollowCursor))]
public class FollowCursorEditor : Editor
{
    private FollowCursor FollowCursor { get { return (target as FollowCursor); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("This component will keep GameObject under cursor and inside bounds", EditorStyles.wordWrappedLabel);
        if (!FollowCursor.SceneBounds)
        {
            var style = new GUIStyle() { fontStyle = FontStyle.Bold, wordWrap = true, normal = new GUIStyleState() { textColor = Color.red } };
            EditorGUILayout.LabelField("This component needs scene bounds! Please assign them.", style);
        }
    }
}
#endif


