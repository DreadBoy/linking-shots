using UnityEngine;

public class Door : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public BoxCollider2D boxCollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (player.transform.position2D() - boxCollider.bounds.center.ToVector2()).magnitude < 1f)
            Toggle();
    }

    private void Toggle()
    {
        animator.SetBool("opened", !animator.GetBool("opened"));
    }

    private void Reset()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
    }
}
