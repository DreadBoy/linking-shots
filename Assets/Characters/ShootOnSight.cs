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

    bool CheckLineOfSight()
    {
        float angle = Vector2.Angle(guard.Forward, guard.player.transform.position2D() - transform.position2D());
        if (angle > 45)
            return false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position2D(), guard.player.transform.position2D(), LayerMask.GetMask(Layers.Walls));
        if (hit)
            Debug.DrawLine(transform.position, hit.point, Color.green);
        else
            Debug.DrawLine(transform.position, guard.player.transform.position2D(), Color.red);
        return !hit;
    }

    void Execute()
    {
        guard.Facing = guard.player.transform.position2D() - transform.position2D();
        float angle = Vector2.SignedAngle(transform.up, guard.Facing);
        guard.rigidBody.MoveRotation(guard.rigidBody.rotation + angle * Time.deltaTime * guard.turnSpeed);
        guard.Shoot();
    }

    public bool Run()
    {
        if (!CheckLineOfSight())
            return false;
        Execute();
        return true;
    }
}
