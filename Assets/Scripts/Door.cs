using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class Door : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public BoxCollider2D boxCollider;
    public InteractionTrigger interactionTrigger;

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionTrigger.keyCode) && (player.transform.position2D() - boxCollider.bounds.center.ToVector2()).magnitude < interactionTrigger.distance)
            Toggle();
    }

    private void Toggle()
    {
        animator.SetBool("opened", !animator.GetBool("opened"));
        UpdateText();
    }

    private void UpdateText()
    {
        if (animator.GetBool("opened"))
            interactionTrigger.text = interactionTrigger.reverseAction;
        else
            interactionTrigger.text = interactionTrigger.action;
    }

    private void Reset()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        interactionTrigger = GetComponent<InteractionTrigger>();
    }
}
