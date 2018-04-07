using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public class Guard : Character
{
    public Player player;
    public Animator animator;

    public Vector2? lastPlayerPosition = null;
    public bool HasLineOfSight { get { return animator.GetBool("hasLineOfSight"); } private set { } }

    void Reset()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        foreach (var state in animator.GetBehaviours<GuardState>())
            state.guard = this;
    }

    protected override void Update()
    {
        base.Update();
        if (Dead)
            return;

        bool hasLineOfSight = CheckLineOfSight();
        if (hasLineOfSight && !animator.GetBool("hasLineOfSight"))
            lastPlayerPosition = null;
        else if (!hasLineOfSight && animator.GetBool("hasLineOfSight"))
            lastPlayerPosition = player.transform.position;
        animator.SetBool("hasLineOfSight", hasLineOfSight);


        var behaviour = animator.GetCurrentBehaviour<GuardState>();
        if (behaviour != null)
            behaviour.Run();
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

    public void RunTowardTarget(Vector2 target)
    {
        rigidBody.MovePosition(transform.position2D() + (target - transform.position2D()).normalized * Time.deltaTime * runSpeed);
    }

    public void EndOfPursue()
    {
        animator.SetTrigger("atLastPlayerPosition");
    }

    public override object GetData()
    {
        return new Data()
        {
            Weapon = weapon,
            Dead = Dead,
            lastPlayerPosition = lastPlayerPosition,
            hasLineOfSight = animator.GetBool("hasLineOfSight"),
            currentState = animator.GetCurrentAnimatorStateInfo(0).fullPathHash,
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
        lastPlayerPosition = d.lastPlayerPosition;
        animator.SetBool("hasLineOfSight", d.hasLineOfSight);
        animator.CrossFade(d.currentState, 0);
    }

    struct Data
    {
        public Weapon Weapon;
        public bool Dead;
        public bool hasLineOfSight;
        public Vector2? lastPlayerPosition;
        public bool inPursueMode;
        public int currentState;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Guard))]
public class GuardEditor : Editor
{
    private Guard Guard { get { return (target as Guard); } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif
