using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(Character))]
public class CharacterRenderer : MonoBehaviour
{
    Character character;
    SpriteRenderer spriteRenderer;

    [HideInInspector]
    public Sprite[] sprites = new Sprite[5];

    Color[] colors = new Color[] { Color.white, new Color(0.8490566f, 0.3804735f, 0.3804735f) };

    void Start()
    {
        character = GetComponent<Character>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        switch (character.weapon.Type)
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
        spriteRenderer.color = character.Dead ? colors[1] : colors[0];
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterRenderer))]
public class CharacterRendererEditor : Editor
{
    private CharacterRenderer CharacterRenderer { get { return (target as CharacterRenderer); } }

    public void OnEnable()
    {
        if (CharacterRenderer.sprites == null || CharacterRenderer.sprites.Length != 5)
        {
            CharacterRenderer.sprites = new Sprite[5];
            EditorUtility.SetDirty(CharacterRenderer);
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        CharacterRenderer.sprites[0] = (Sprite)EditorGUILayout.ObjectField("Idle", CharacterRenderer.sprites[0], typeof(Sprite), false, null);
        CharacterRenderer.sprites[1] = (Sprite)EditorGUILayout.ObjectField("Hand", CharacterRenderer.sprites[1], typeof(Sprite), false, null);
        CharacterRenderer.sprites[2] = (Sprite)EditorGUILayout.ObjectField("Gun", CharacterRenderer.sprites[2], typeof(Sprite), false, null);
        CharacterRenderer.sprites[3] = (Sprite)EditorGUILayout.ObjectField("Riffle", CharacterRenderer.sprites[3], typeof(Sprite), false, null);
        CharacterRenderer.sprites[4] = (Sprite)EditorGUILayout.ObjectField("Shotgun", CharacterRenderer.sprites[4], typeof(Sprite), false, null);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Changed sprite");
    }
}
#endif
