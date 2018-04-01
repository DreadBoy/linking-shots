using UnityEngine;
using System.Linq;
using System;
using UnityEditorInternal;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Guard : Character
{
    public Player player;
    public ICharacterComponent[] components;

    void Reset()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void Start()
    {
        base.Start();
        components = GetComponents<ICharacterComponent>().OrderByDescending(i => i.Priority).ToArray();
    }

    protected override void Update()
    {
        base.Update();
        if (Dead)
            return;
        foreach (var comp in components)
            if (comp.Run())
                break;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Guard))]
public class GuardEditor : Editor
{
    private Guard Guard { get { return (target as Guard); } }
    int selectedIndex = 0;
    Type[] components = InterfaceSearch.GetTypesWithThisInterface<ICharacterComponent>().ToArray();
    string[] componentsString = new string[] { "Select component" }.Concat(InterfaceSearch.GetTypesWithThisInterface<ICharacterComponent>().Select(t => t.ToString())).ToArray();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginVertical();
        GUILayout.Label("Add component", new GUIStyle() { fontStyle = FontStyle.Bold });
        int selected = EditorGUILayout.Popup(selectedIndex, componentsString);
        if (selectedIndex != selected)
            Undo.AddComponent(Guard.gameObject, components.ElementAt(selected - 1));
        GUILayout.Label("Actions", new GUIStyle() { fontStyle = FontStyle.Bold });
        GUILayout.BeginHorizontal();
        GUILayout.Label("Priority");
        GUILayout.Label("Action");
        GUILayout.EndHorizontal();
        foreach (ICharacterComponent action in Guard.GetComponents<ICharacterComponent>().OrderByDescending(i => i.Priority))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(action.Priority.ToString(), new GUIStyle() { fontStyle = FontStyle.Bold });
            GUILayout.Label((action as Component).GetType().ToString());
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
#endif
