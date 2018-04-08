using UnityEngine;

public class Door : MonoBehaviour
{
    public Player player;
    public Animator animator; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (player.transform.position2D() - transform.position2D()).magnitude < 0.5f)
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
    }
}
