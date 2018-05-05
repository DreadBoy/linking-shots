using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WeaponInfo : MonoBehaviour
{
    public Player player;
    [SerializeField]
    public WeaponPair[] weaponPairs = new WeaponPair[0];
    public Text weaponName;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!player || !weaponName)
            return;
        foreach (var pair in weaponPairs)
            pair.gameObject.gameObject.SetActive(player.weapon.Type == pair.weapon.Type);
        weaponName.text = player.weapon.Type.ToString() + " (" + getWeaponAmmo(player.weapon) + ")";
    }

    string getWeaponAmmo(Weapon weapon)
    {
        if (weapon.Type == WeaponType.Hand)
            return "∞";
        else
            return weapon.Ammo.ToString();
    }

    [Serializable]
    public struct WeaponPair
    {
        public Weapon weapon;
        public RectTransform gameObject;
    }
}
