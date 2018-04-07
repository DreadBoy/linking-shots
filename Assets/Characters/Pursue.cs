using UnityEngine;

public class Pursue : GuardState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Run()
    {
        if (!guard.HasLineOfSight && guard.lastPlayerPosition.HasValue)
        {
            if ((guard.lastPlayerPosition.Value - guard.transform.position2D()).magnitude < 0.1)
            {
                guard.rigidBody.velocity = Vector2.zero;
                guard.lastPlayerPosition = null;
                guard.EndOfPursue();
                return;
            }
            guard.FaceTowardTarget(guard.lastPlayerPosition.Value);
            guard.RunTowardTarget(guard.lastPlayerPosition.Value);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
