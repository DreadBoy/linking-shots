using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

[RequireComponent(typeof(TimeshiftEvents))]
[RequireComponent(typeof(AudioSource))]
public class TimeshiftMusic : MonoBehaviour
{
    public TimeshiftEvents timeshiftEvents;
    public AudioSource source;
    public TimeMaster timeMaster;

    void Reset()
    {
        timeshiftEvents = GetComponent<TimeshiftEvents>();
        if (!timeshiftEvents)
            timeshiftEvents = gameObject.AddComponent<TimeshiftEvents>();
        source = GetComponent<AudioSource>();
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
#if UNITY_EDITOR
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStart, TimeshiftStart);
        UnityEventTools.AddPersistentListener(timeshiftEvents.TimeshiftStop, TimeshiftStop);
#endif
        timeMaster = FindObjectOfType<TimeMaster>();
    }

    public void TimeshiftStart()
    {
        source.pitch = -timeMaster.rewindSpeed;
    }

    public void TimeshiftStop()
    {
        source.pitch = 1;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TimeshiftMusic))]
public class TimeshiftMusicEditor : Editor
{
    private TimeshiftMusic TimeshiftMusic { get { return (target as TimeshiftMusic); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("This component will reverse music when world is in timeshift. Don't forget to include Timeshift Events component if it isn't already!", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("You can reset this component to add listeners to Timeshift Events automatically. ;)", EditorStyles.wordWrappedLabel);
        if (!TimeshiftMusic.GetComponent<TimeshiftEvents>())
        {
            var style = new GUIStyle() { fontStyle = FontStyle.Bold, wordWrap = true, normal = new GUIStyleState() { textColor = Color.red } };
            EditorGUILayout.LabelField("Timeshift Music needs Timeshift Events component, please add it!", style);
        }
    }
}
#endif

