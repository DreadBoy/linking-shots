public class ShootOnSight : GuardState
{
    public override void Run()
    {
        if (!guard.HasLineOfSight)
            return;
        guard.FaceTowardTarget(guard.player.transform.position2D());
        guard.Shoot();
    }
}
