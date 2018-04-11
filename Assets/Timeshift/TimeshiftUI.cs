using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

[RequireComponent(typeof(TimeshiftEvents))]
public class TimeshiftUI : MonoBehaviour
{

    TimeshiftEvents timeshiftEvents;
    public Image image;

    public void TimeshiftStart()
    {
        image.enabled = true;
    }

    public void TimeshiftStop()
    {
        image.enabled = false;
    }

    void Reset()
    {
        timeshiftEvents = GetComponent<TimeshiftEvents>();
        if (!timeshiftEvents)
            timeshiftEvents = gameObject.AddComponent<TimeshiftEvents>();
        image = GetComponent<Image>();
#if UNITY_EDITOR
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStart, TimeshiftStart);
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStop, TimeshiftStop);
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TimeshiftUI))]
public class TimeshiftUIEditor : Editor
{
    private TimeshiftUI TimeshiftUI { get { return (target as TimeshiftUI); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("This component will add rewing icon to UI when world is in timeshift. Don't forget to include Timeshift Events component if it isn't already!", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("You can reset this component to add listeners to Timeshift Events automatically. ;)", EditorStyles.wordWrappedLabel);
        if (!TimeshiftUI.GetComponent<TimeshiftEvents>())
        {
            var style = new GUIStyle() { fontStyle = FontStyle.Bold, wordWrap = true, normal = new GUIStyleState() { textColor = Color.red } };
            EditorGUILayout.LabelField("Timeshift UI needs Timeshift Events component, please add it!", style);
        }
    }
}
#endif


