﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pickup : MonoBehaviour
{
    public Weapon weapon;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        if (!weapon) {
            Debug.Log("Assign Weapon object!");
            return;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.Sprite;
    }

    public Weapon Pick(Weapon weapon)
    {
        if (weapon.Type == WeaponType.Hand)
        {
            gameObject.SetActive(false);
            return Instantiate(this.weapon);
        }
        return weapon;
    }
}
