using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TimeDependant : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        FindObjectOfType<TimeMaster>().TrackObject(this);
    }

    object GetData()
    {
        return GetComponents<IAffectedByTime>().Select(d => new KeyValuePair<int, object>(d.GetHashCode(), d.GetData())).ToArray();
    }

    void SetData(object data)
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

    public TimeMaster.ObjectInstant GetInstant()
    {
        if (animator != null)
            return new TimeMaster.ObjectInstant(GetInstanceID(),
                gameObject.activeSelf, transform.position, transform.rotation,
                animator.GetCurrentAnimatorStateInfo(0).fullPathHash, animator.parameters.Select(p => new TimeMaster.AnimatorParameterInstant(animator, p)).ToArray(),
                GetData());
        return new TimeMaster.ObjectInstant(GetInstanceID(),
            gameObject.activeSelf, transform.position, transform.rotation,
            GetData());
    }

    public void SetInstant(TimeMaster.ObjectInstant instant)
    {
        transform.position = instant.position;
        transform.rotation = instant.rotation;
        // activeSelf is set by TimeMaster
        if (animator != null)
        {
            animator.CrossFade(instant.animatorState, 0);
            foreach (TimeMaster.AnimatorParameterInstant parameter in instant.animatorParameters)
            {
                AnimatorControllerParameter param = animator.parameters.FirstOrDefault(p => p.nameHash == parameter.nameHash);
                if (param != null)
                    animator.SetParameter(parameter);
            }
        }
        SetData(instant.data);
    }

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(TimeDependant))]
public class TimeDependantEditor : Editor
{
    private TimeDependant TimeDependant { get { return (target as TimeDependant); } }
    SerializedProperty animator;

    void OnEnable()
    {
        animator = serializedObject.FindProperty("animator");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("This component will track position, rotation, isActive and any custom data of this GameObject.", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("If Animator is attached to object, this component will track current state and value of all parameters.", EditorStyles.wordWrappedLabel);
        Component[] comps = TimeDependant.GetComponents<IAffectedByTime>().Cast<Component>().ToArray();
        if (comps.Length > 0)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Recognised components with custom data:", EditorStyles.boldLabel);
            foreach (Component comp in comps)
                EditorGUILayout.LabelField(comp.GetType().ToString());
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.LabelField("Optional animator:", EditorStyles.boldLabel);
        serializedObject.Update();
        EditorGUILayout.PropertyField(animator);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

