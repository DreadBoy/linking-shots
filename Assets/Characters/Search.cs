using UnityEngine;

public class Search : GuardState
{
    public override void Run(Guard guard)
    {
        if (!guard.HasLineOfSight && !guard.lastPlayerPosition.HasValue)
        {
            guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
            if (Random.Range(0, 19) != 0)
                return;
            guard.FaceTowardTarget(guard.Forward * 5 + guard.Right * Random.Range(-0.3f, 0.3f));
            guard.WalkTowardTarget(guard.Forward * 5 + guard.Right * Random.Range(-0.3f, 0.3f));
        }
        else if (guard.rigidBody.bodyType != RigidbodyType2D.Kinematic)
            guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
