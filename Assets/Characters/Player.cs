using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    new Camera camera;

    Vector2 mousePosition;
    Vector2 facing;

    float lastShot = 0;

    protected override void Start()
    {
        base.Start();
        camera = FindObjectOfType<Camera>();
    }

    protected override void Update()
    {
        base.Update();
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            direction += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            direction += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            direction += Vector2.right;

        rigidBody.MovePosition((Vector2)transform.position + direction * Time.deltaTime * moveSpeed);

        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        facing = mousePosition - (Vector2)transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(facing, Vector3.back);
        rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed).eulerAngles.z);

        CheckShoot();
    }

    void CheckShoot()
    {
        Debug.DrawRay(transform.position, facing, Color.green);
        if (Input.GetMouseButton(0))
        {
            if (weapon == Weapon.Riffle && Time.time - lastShot < 1)
                return;


        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : CharacterEditor
{
}
#endif