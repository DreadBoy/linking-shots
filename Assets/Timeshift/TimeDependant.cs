using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TimeDependant : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<TimeMaster>().TrackObject(this);
    }

    public object GetData()
    {
        return GetComponents<IAffectedByTime>().Select(d => new KeyValuePair<int, object>(d.GetHashCode(), d.GetData())).ToArray();
    }

    public void SetData(object data)
    {
        if (data == null)
            return;
        IAffectedByTime[] components = GetComponents<IAffectedByTime>();
        KeyValuePair<int, object>[] dict = data as KeyValuePair<int, object>[];
        foreach (var pair in dict)
        {
            IAffectedByTime component = components.FirstOrDefault(c => c.GetHashCode() == pair.Key);
            if (component != null)
                component.SetData(pair.Value);
        }
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(TimeDependant))]
public class TimeDependantEditor : Editor
{
    private TimeDependant TimeDependant { get { return (target as TimeDependant); } }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("This component will track position, rotation, isActive and any custom data of this GameObject.", EditorStyles.wordWrappedLabel);
        Component[] comps = TimeDependant.GetComponents<IAffectedByTime>().Cast<Component>().ToArray();
        if(comps.Length == 0)
            return;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Recognised components with custom data:", EditorStyles.boldLabel);
        foreach (Component comp in comps)
            EditorGUILayout.LabelField(comp.GetType().ToString());
        EditorGUILayout.EndVertical();
    }
}
#endif

