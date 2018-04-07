using UnityEngine;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Guard : Character
{
    public Player player;
    public ICharacterComponent[] components;

    public bool hasLineOfSight = false;
    public Vector2? lastPlayerPosition = null;
    public bool inPursueMode = false;
    

    void Reset()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void Start()
    {
        base.Start();
        components = GetComponents<ICharacterComponent>().OrderByDescending(i => i.Priority).ToArray();
    }

    protected override void Update()
    {
        base.Update();
        bool hasLineOfSight = CheckLineOfSight();
        if (hasLineOfSight && !this.hasLineOfSight)
            lastPlayerPosition = null;
        else if (!hasLineOfSight && this.hasLineOfSight)
            lastPlayerPosition = player.transform.position;
        this.hasLineOfSight = hasLineOfSight;

        if (Dead)
            return;
        foreach (var comp in components)
            if (comp.Run())
                break;
    }

    bool CheckLineOfSight()
    {
        float angle = Vector2.Angle(Forward, player.transform.position2D() - transform.position2D());
        if (angle > 45)
            return false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position2D(), player.transform.position2D(), LayerMask.GetMask(Layers.Walls));
        if (hit)
            Debug.DrawLine(transform.position, hit.point, Color.green);
        else
            Debug.DrawLine(transform.position, player.transform.position2D(), Color.red);
        return !hit;
    }

    public void FaceTowardTarget(Vector2 target)
    {
        Facing = target - transform.position2D();
        float angle = Vector2.SignedAngle(transform.up, Facing);
        rigidBody.MoveRotation(rigidBody.rotation + angle * Time.deltaTime * turnSpeed);
    }

    public void WalkTowardTarget(Vector2 target)
    {
        rigidBody.MovePosition(transform.position2D() + (target - transform.position2D()).normalized * Time.deltaTime * moveSpeed);
    }

    public override object GetData()
    {
        return new Data()
        {
            Weapon = weapon,
            Dead = Dead,
            hasLineOfSight = hasLineOfSight,
            lastPlayerPosition = lastPlayerPosition,
            inPursueMode = inPursueMode,
        };
    }

    public override void SetData(object data)
    {
        if (!(data is Data))
            return;
        Data d = (Data)data;
        weapon = d.Weapon;
        Dead = d.Dead;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = !Dead;
        hasLineOfSight = d.hasLineOfSight;
        lastPlayerPosition = d.lastPlayerPosition;
        inPursueMode = d.inPursueMode;
    }

    struct Data
    {
        public Weapon Weapon;
        public bool Dead;
        public bool hasLineOfSight;
        public Vector2? lastPlayerPosition;
        public bool inPursueMode;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Guard))]
public class GuardEditor : Editor
{
    private Guard Guard { get { return (target as Guard); } }
    int selectedIndex = 0;
    Type[] components = InterfaceSearch.GetTypesWithThisInterface<ICharacterComponent>().ToArray();
    string[] componentsString = new string[] { "Select component" }.Concat(InterfaceSearch.GetTypesWithThisInterface<ICharacterComponent>().Select(t => t.ToString())).ToArray();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginVertical();
        GUILayout.Label("Add component", new GUIStyle() { fontStyle = FontStyle.Bold });
        int selected = EditorGUILayout.Popup(selectedIndex, componentsString);
        if (selectedIndex != selected)
            Undo.AddComponent(Guard.gameObject, components.ElementAt(selected - 1));
        GUILayout.Label("Actions", new GUIStyle() { fontStyle = FontStyle.Bold });
        GUILayout.BeginHorizontal();
        GUILayout.Label("Priority");
        GUILayout.Label("Action");
        GUILayout.EndHorizontal();
        foreach (ICharacterComponent action in Guard.GetComponents<ICharacterComponent>().OrderByDescending(i => i.Priority))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(action.Priority.ToString(), new GUIStyle() { fontStyle = FontStyle.Bold });
            GUILayout.Label((action as Component).GetType().ToString());
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
#endif
