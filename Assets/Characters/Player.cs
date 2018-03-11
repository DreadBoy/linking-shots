using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    new Camera camera;

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

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        facing = mousePosition - transform.position2D();
        float angle = Vector2.SignedAngle(transform.up, facing);

        rigidBody.MoveRotation(rigidBody.rotation + angle * Time.deltaTime * turnSpeed);

        CheckShoot();
    }

    void CheckShoot()
    {
        switch (weapon)
        {
            case Weapon.Gun:
                if (Input.GetMouseButtonDown(0) && shooting == false && Time.time - lastShot >= 0.5)
                    Shoot();
                break;
            case Weapon.Hand:
            case Weapon.Shotgun:
                if (Input.GetMouseButtonDown(0) && shooting == false && Time.time - lastShot >= 1)
                    Shoot();
                break;
            case Weapon.Riffle:
                if (Input.GetMouseButton(0) && Time.time - lastShot >= 0.1f)
                    Shoot();
                break;
        }
        if (Input.GetMouseButtonDown(0))
            shooting = true;
        else if(Input.GetMouseButtonUp(0))
            shooting = false;

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : CharacterEditor
{
}
#endif