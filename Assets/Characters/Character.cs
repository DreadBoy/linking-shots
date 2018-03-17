﻿using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected float moveSpeed = 8, turnSpeed = 60;
    [HideInInspector]
    public Sprite[] sprites = new Sprite[5];
    [SerializeField]
    Pellet pelletPrefab;
    [SerializeField]
    GameObject bloodPrefab;

    public Weapon weapon;
    protected bool shooting = false;
    protected float lastShot = 0;

    public Vector2 Forward { get { return transform.up; } private set { } }
    private Vector2 _facing;
    public Vector2 Facing { get { return _facing; } set { _facing = value; } }
    public bool Dead = false;


    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!pelletPrefab)
            Debug.LogError("Assign pellet prefab!");
        if (!bloodPrefab)
            Debug.LogError("Assign blood prefab!");
        if(!weapon)
        {
            Debug.LogWarning("Weapon isn't assigned, creating empty hand!");
            weapon = ScriptableObject.CreateInstance<Weapon>();
            weapon.Type = WeaponType.Hand;
        }
    }

    protected virtual void Update()
    {
        switch (weapon.Type)
        {
            case WeaponType.Hand:
                spriteRenderer.sprite = sprites[0];
                break;
            case WeaponType.Gun:
                spriteRenderer.sprite = sprites[2];
                break;
            case WeaponType.Riffle:
                spriteRenderer.sprite = sprites[3];
                break;
            case WeaponType.Shotgun:
                spriteRenderer.sprite = sprites[4];
                break;
        }
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

    protected virtual void Shoot()
    {
        switch (weapon.Type)
        {
            case WeaponType.Gun:
                if (Time.time - lastShot < 0.5)
                    return;
                break;
            case WeaponType.Hand:
            case WeaponType.Shotgun:
                if (Time.time - lastShot < 1)
                    return;
                break;
            case WeaponType.Riffle:
                if (Time.time - lastShot < 0.1f)
                    return;
                break;
        }
        switch (weapon.Type)
        {
            case WeaponType.Gun:
                CreateShot(new Vector2(0.15f, 0.46f), Vector2.zero);
                break;
            case WeaponType.Riffle:
                CreateShot(new Vector2(0.15f, 0.54f), Vector2.zero);
                break;
            case WeaponType.Shotgun:
                CreateShot(new Vector2(0.15f, 0.46f), Vector2.zero);
                CreateShot(new Vector2(0.15f, 0.46f), transform.up + transform.right * 0.5f);
                CreateShot(new Vector2(0.15f, 0.46f), transform.up - transform.right * 0.5f);
                break;
        }
        lastShot = Time.time;
    }

    public virtual void GetKilled(Vector2 shotDirection)
    {
        Dead = true;
        GameObject blood = Instantiate(bloodPrefab);
        blood.transform.position = transform.position;
        blood.transform.up = shotDirection;
        spriteRenderer.color = new Color(0.8490566f, 0.3804735f, 0.3804735f);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    private Character Character { get { return (target as Character); } }

    public void OnEnable()
    {
        if (Character.sprites == null || Character.sprites.Length != 5)
        {
            Character.sprites = new Sprite[5];
            EditorUtility.SetDirty(Character);
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        Character.sprites[0] = (Sprite)EditorGUILayout.ObjectField("Idle", Character.sprites[0], typeof(Sprite), false, null);
        Character.sprites[1] = (Sprite)EditorGUILayout.ObjectField("Hand", Character.sprites[1], typeof(Sprite), false, null);
        Character.sprites[2] = (Sprite)EditorGUILayout.ObjectField("Gun", Character.sprites[2], typeof(Sprite), false, null);
        Character.sprites[3] = (Sprite)EditorGUILayout.ObjectField("Riffle", Character.sprites[3], typeof(Sprite), false, null);
        Character.sprites[4] = (Sprite)EditorGUILayout.ObjectField("Shotgun", Character.sprites[4], typeof(Sprite), false, null);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Changed sprite");
    }
}
#endif