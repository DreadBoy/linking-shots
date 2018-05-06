using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractionTrigger : MonoBehaviour
{

    public Transform target;
    public float distance;
    public BoxCollider2D boxCollider;
    public KeyCode keyCode;
    public string action;
    public string reverseAction;
    [HideInInspector]
    public string text;

    public Vector2 GetCenter()
    {
        if (boxCollider)
            return boxCollider.bounds.center.ToVector2();
        return target.position2D();
    }

    private void Reset()
    {
        target = GetComponent<Transform>();
        distance = 1;
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        keyCode = KeyCode.E;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InteractionTrigger))]
public class InteractionTriggerEditor : Editor
{
    private InteractionTrigger InteractionTrigger { get { return (target as InteractionTrigger); } }
    bool displayCircle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Interaction trigger tells Interaction Popup component when to display popup", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("Assign it transform or optionally box collider. If box collider is assigned, component will use center of collider instead of transform's position", EditorStyles.wordWrappedLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Display circle");
        displayCircle = EditorGUILayout.Toggle(displayCircle);
        EditorGUILayout.EndHorizontal();

    }

    public void OnSceneGUI()
    {
        if (displayCircle)
            Handles.DrawWireArc(InteractionTrigger.GetCenter(), Vector3.back, InteractionTrigger.transform.position + Vector3.up * InteractionTrigger.distance, 360, InteractionTrigger.distance);
    }
}
#endif