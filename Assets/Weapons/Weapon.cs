using UnityEngine;

public enum WeaponType
{
    Hand,
    Gun,
    Riffle,
    Shotgun
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/Weapon", order = 2)]
public class Weapon : ScriptableObject
{
    public WeaponType Type;
    public int Ammo;
    public Sprite Sprite;
}