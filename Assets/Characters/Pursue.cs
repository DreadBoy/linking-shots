using UnityEngine;

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
            guard.inPursueMode = true;
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
