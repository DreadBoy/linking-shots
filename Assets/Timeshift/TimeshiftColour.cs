using UnityEngine;
using UnityEditor.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TimeshiftEvents))]
public class TimeshiftColour : MonoBehaviour
{

    [HideInInspector]
    public Material normalMaterial;
    public Material reverseMaterial;

    new Renderer renderer;
    TimeshiftEvents timeshiftEvents;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        timeshiftEvents = GetComponent<TimeshiftEvents>();
    }

    public void TimeshiftStart()
    {
        normalMaterial = renderer.material;
        renderer.material = GetMaterialForTimeshift();
    }

    public void TimeshiftStop()
    {
        renderer.material = normalMaterial;
    }

    Material GetMaterialForTimeshift()
    {
        if (reverseMaterial)
            return reverseMaterial;
        else
            return normalMaterial;
    }

    void Reset()
    {
        timeshiftEvents = GetComponent<TimeshiftEvents>();
        if (!timeshiftEvents)
            timeshiftEvents = gameObject.AddComponent<TimeshiftEvents>();
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStart, TimeshiftStart);
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStop, TimeshiftStop);
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(TimeshiftColour))]
public class TimeshiftColourEditor : Editor
{
    private TimeshiftColour TimeshiftColour { get { return (target as TimeshiftColour); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("This component will change shader when world is in timeshift. Don't forget to include Timeshift Events component if it isn't already!", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("You can reset this component to add listeners to Timeshift Events automatically. ;)", EditorStyles.wordWrappedLabel);
        if (!TimeshiftColour.GetComponent<TimeshiftEvents>())
        {
            var style = new GUIStyle() { fontStyle = FontStyle.Bold, wordWrap = true, normal = new GUIStyleState() { textColor = Color.red } };
            EditorGUILayout.LabelField("Timeshift Colour needs Timeshift Events component, please add it!", style);
        }
    }
}
#endif

