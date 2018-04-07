public class ShootOnSight : GuardState
{
    public override void Run(Guard guard)
    {
        if (!guard.HasLineOfSight)
            return;
        guard.FaceTowardTarget(guard.player.transform.position2D());
        guard.Shoot();
    }
}
