using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Data/GameState", order = 1)]
public class GameState : ScriptableObject {
    public string LoadedScene;
    public Weapon Weapon;
}
