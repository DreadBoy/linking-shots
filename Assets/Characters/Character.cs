using UnityEngine;

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
    public Sprite[] sprites = new Sprite[0];

    [SerializeField]
    protected Weapon weapon = Weapon.Hand;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        switch(weapon)
        {
            case Weapon.Hand:
                spriteRenderer.sprite = sprites[0];
                break;
            case Weapon.Gun:
                spriteRenderer.sprite = sprites[2];
                break;
            case Weapon.Riffle:
                spriteRenderer.sprite = sprites[3];
                break;
            case Weapon.Shotgun:
                spriteRenderer.sprite = sprites[4];
                break;

        }
    }

    protected virtual void Shoot()
    {

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    private Character Character { get { return (target as Character); } }

    public void OnEnable()
    {
        if (Character.sprites == null || Character.sprites.Length != 6)
        {
            Character.sprites = new Sprite[6];
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
        Character.sprites[5] = (Sprite)EditorGUILayout.ObjectField("Shotgun shooting", Character.sprites[5], typeof(Sprite), false, null);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(Character);
    }
}
#endif