using UnityEngine;

[RequireComponent(typeof(Guard))]
public class Search : MonoBehaviour, ICharacterComponent
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
        if (!guard.hasLineOfSight && !guard.lastPlayerPosition.HasValue && guard.inPursueMode)
        {
            guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
            if (Random.Range(0, 19) != 0)
                return false;
            guard.FaceTowardTarget(guard.Forward * 5 + guard.Right * Random.Range(-0.3f, 0.3f));
            guard.WalkTowardTarget(guard.Forward * 5 + guard.Right * Random.Range(-0.3f, 0.3f));
            return true;
        }
        else if (guard.rigidBody.bodyType != RigidbodyType2D.Kinematic)
            guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
        return false;
    }
}
