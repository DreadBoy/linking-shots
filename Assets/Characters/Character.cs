﻿using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour, IAffectedByTime
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    
    public float moveSpeed = 2, runSpeed = 8, turnSpeed = 60;
    [SerializeField]
    Pellet pelletPrefab;
    [SerializeField]
    GameObject bloodPrefab;

    public Weapon weapon;
    protected bool shooting = false;
    protected float lastShot = 0;

    public Vector2 Forward { get { return transform.up; } private set { } }
    public Vector2 Right { get { return -transform.right; } private set { } }
    private Vector2 _facing;
    public Vector2 Facing { get { return _facing; } set { _facing = value; } }
    private bool _dead;
    public bool Dead { get { return _dead; } set { _dead = value; } }

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!pelletPrefab)
            Debug.LogError("Assign pellet prefab!");
        if (!bloodPrefab)
            Debug.LogError("Assign blood prefab!");
        if (!weapon)
        {
            Debug.LogWarning("Weapon isn't assigned, creating empty hand!");
            weapon = ScriptableObject.CreateInstance<Weapon>();
            weapon.Type = WeaponType.Hand;
        }
    }

    protected virtual void Update()
    {

    }

    protected Pickup[] GetPickups()
    {
        return Physics2D.OverlapCircleAll(transform.position2D(), 0.5f)
            .Select(collider => collider.GetComponent<Pickup>())
            .Where(pickup => !!pickup)
            .ToArray();
    }

    protected void Pickup()
    {
        Pickup pickup = GetPickups().FirstOrDefault();
        if (!pickup)
            return;
        if (weapon.Type == WeaponType.Hand)
            weapon = pickup.Pick(weapon);
    }

    void CreateShot(Vector2 origin, Vector2 relativeDirection)
    {
        Pellet pellet = Instantiate(pelletPrefab);
        pellet.name = "Pellet";
        pellet.transform.position = transform.position + transform.rotation * origin;
        pellet.direction = Facing + relativeDirection;
    }

    public virtual void Shoot()
    {
        switch (weapon.Type)
        {
            case WeaponType.Gun:
                if (Time.time - lastShot < 0.5 || weapon.Ammo == 0)
                    return;
                break;
            case WeaponType.Hand:
                if (Time.time - lastShot < 1)
                    return;
                break;
            case WeaponType.Shotgun:
                if (Time.time - lastShot < 1 || weapon.Ammo < 3)
                    return;
                break;
            case WeaponType.Riffle:
                if (Time.time - lastShot < 0.1f || weapon.Ammo == 0)
                    return;
                break;
        }
        switch (weapon.Type)
        {
            case WeaponType.Gun:
                CreateShot(new Vector2(0.15f, 0.46f), Vector2.zero);
                weapon.Ammo--;
                break;
            case WeaponType.Riffle:
                CreateShot(new Vector2(0.15f, 0.54f), Vector2.zero);
                weapon.Ammo--;
                break;
            case WeaponType.Shotgun:
                CreateShot(new Vector2(0.15f, 0.46f), Vector2.zero);
                CreateShot(new Vector2(0.15f, 0.46f), transform.up + transform.right * 0.5f);
                CreateShot(new Vector2(0.15f, 0.46f), transform.up - transform.right * 0.5f);
                weapon.Ammo -= 3;
                break;
        }
        lastShot = Time.time;
    }

    public virtual void GetKilled(Vector2 shotDirection)
    {
        Dead = true;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = !Dead;
        GameObject blood = Instantiate(bloodPrefab);
        blood.transform.position = transform.position;
        blood.transform.up = shotDirection;
    }

    public virtual object GetData()
    {
        return new Data()
        {
            Weapon = weapon,
            Dead = Dead,
        };
    }

    public virtual void SetData(object data)
    {
        if (!(data is Data))
            return;
        weapon = ((Data)data).Weapon;
        Dead = ((Data)data).Dead;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = !Dead;
    }

    struct Data
    {
        public Weapon Weapon;
        public bool Dead;
    }

    public bool Enabled { get { return enabled; } set { enabled = value; } }
}