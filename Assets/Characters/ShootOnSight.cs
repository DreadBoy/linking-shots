using UnityEngine;

[RequireComponent(typeof(Guard))]
public class ShootOnSight : MonoBehaviour, ICharacterComponent
{
    public Guard guard;
    [SerializeField]
    int priority = 0;
    public int Priority { get { return priority; } set { } }

    void Reset()
    {
        guard = GetComponent<Guard>();
    }

    void Execute()
    {
        guard.FaceTowardTarget(guard.player.transform.position2D());
        guard.Shoot();
    }

    public bool Run()
    {
        if (!guard.hasLineOfSight)
            return false;
        Execute();
        return true;
    }
}
