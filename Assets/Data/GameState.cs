using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "GameState", order = 2)]
public class GameState : ScriptableObject {
    public string LoadedScene;
    public Weapon Weapon;
}
