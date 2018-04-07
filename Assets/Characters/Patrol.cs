using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Guard))]
public class Patrol : MonoBehaviour, ICharacterComponent
{
    public Guard guard;
    [SerializeField]
    int priority = 0;
    public int Priority { get { return priority; } set { } }

    void Reset()
    {
        guard = GetComponent<Guard>();
    }

    public bool Run()
    {
        return false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Patrol))]
public class PatrolEditor : Editor
{
    private Patrol Patrol { get { return (target as Patrol); } }

}
#endif