using UnityEngine;

public enum WeaponType
{
    Hand,
    Gun,
    Riffle,
    Shotgun
}

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public WeaponType Type;
    public int Ammo;
    public Sprite Sprite;
}