using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Guard))]
public class Pursue : MonoBehaviour, ICharacterComponent
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
        if (!guard.hasLineOfSight && guard.lastPlayerPosition.HasValue)
        {
            guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
            if ((guard.lastPlayerPosition.Value - guard.transform.position2D()).magnitude < 0.1)
            {
                guard.rigidBody.velocity = Vector2.zero;
                guard.lastPlayerPosition = null;
                guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
                return false;
            }
            guard.FaceTowardTarget(guard.lastPlayerPosition.Value);
            guard.WalkTowardTarget(guard.lastPlayerPosition.Value);
            return true;
        }
        else if (guard.rigidBody.bodyType != RigidbodyType2D.Kinematic)
            guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
        return false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Pursue))]
public class PursueEditor : Editor
{
    private Pursue Pursue { get { return (target as Pursue); } }

}
#endif
