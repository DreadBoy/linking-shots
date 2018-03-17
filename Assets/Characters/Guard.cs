using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Guard : Character
{
    Player player;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
    }

    protected override void Update()
    {
        base.Update();
        if (CheckLineOfSight())
        {
            Facing = player.transform.position2D() - transform.position2D();
            float angle = Vector2.SignedAngle(transform.up, Facing);
            rigidBody.MoveRotation(rigidBody.rotation + angle * Time.deltaTime * turnSpeed);
            Shoot();
        }

    }

    bool CheckLineOfSight()
    {
        float angle = Vector2.Angle(Forward, player.transform.position2D() - transform.position2D());
        Debug.Log(angle);
        if (angle > 45)
            return false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position2D(), player.transform.position2D(), LayerMask.GetMask(Layers.Walls));
        if (hit)
            Debug.DrawLine(transform.position, hit.point, Color.green);
        else
            Debug.DrawLine(transform.position, player.transform.position2D(), Color.red);
        return !hit;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Guard))]
public class GuardEditor : CharacterEditor
{
}
#endif