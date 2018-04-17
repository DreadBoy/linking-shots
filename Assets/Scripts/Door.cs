using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class Door : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public BoxCollider2D boxCollider;
    public InteractionTrigger interactionTrigger;

    private void Update()
    {
        if (Input.GetKeyDown(interactionTrigger.keyCode) && (player.transform.position2D() - boxCollider.bounds.center.ToVector2()).magnitude < interactionTrigger.distance)
            Toggle();
        if (animator.GetBool("opened"))
            interactionTrigger.action = "close door";
        else
            interactionTrigger.action = "open door";
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
        interactionTrigger = GetComponent<InteractionTrigger>();
    }
}
