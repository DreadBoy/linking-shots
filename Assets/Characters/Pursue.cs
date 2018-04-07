using UnityEngine;

public class Pursue : GuardState
{
    public override void Run(Guard guard)
    {
        if (!guard.HasLineOfSight && guard.lastPlayerPosition.HasValue)
        {
            guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
            if ((guard.lastPlayerPosition.Value - guard.transform.position2D()).magnitude < 0.1)
            {
                guard.rigidBody.velocity = Vector2.zero;
                guard.lastPlayerPosition = null;
                guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
                guard.EndOfPursue();
                return;
            }
            guard.FaceTowardTarget(guard.lastPlayerPosition.Value);
            guard.WalkTowardTarget(guard.lastPlayerPosition.Value);
        }
        else if (guard.rigidBody.bodyType != RigidbodyType2D.Kinematic)
            guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
