using System.Linq;
using UnityEngine;

public class Search : GuardState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        guard.rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Run()
    {
        if (!guard.HasLineOfSight)
        {
            Player player = Physics2D.CircleCastAll(guard.transform.position2D(), 10, guard.Forward, LayerMask.GetMask(Layers.Characters))
                .Select(hit => hit.transform.GetComponent<Player>())
                .Where(p => p != null)
                .FirstOrDefault();
            if (player != null)
            {
                guard.FaceTowardTarget(player.transform.position2D());
                guard.WalkTowardTarget(player.transform.position2D());
                return;
            }
            Vector2 target = guard.transform.position2D() + guard.Forward * 5 + guard.Right * Random.Range(-1f, 1f);
            guard.FaceTowardTarget(target);
            guard.WalkTowardTarget(target);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        guard.rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
