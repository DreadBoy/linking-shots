using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Character
{
    protected Vector2 mousePosition;

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
        Facing = mousePosition - transform.position2D();
        float angle = Vector2.SignedAngle(transform.up, Facing);

        rigidBody.MoveRotation(rigidBody.rotation + angle * Time.deltaTime * turnSpeed);
        
        TryShoot();
        Pickup();
    }

    void TryShoot()
    {
        switch (weapon.Type)
        {
            case WeaponType.Gun:
            case WeaponType.Hand:
            case WeaponType.Shotgun:
                if (Input.GetMouseButtonDown(0))
                    Shoot();
                break;
            case WeaponType.Riffle:
                if (Input.GetMouseButton(0))
                    Shoot();
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : CharacterEditor
{
}
#endif