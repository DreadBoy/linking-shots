using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Pellet : MonoBehaviour
{

    public Vector2 direction = Vector2.zero;
    public float speed = 20;

    TrailRenderer trailRenderer;

    Vector2 origin;
    RaycastHit2D hit;


    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.time /= speed;

        direction.Normalize();

        origin = transform.position2D();

        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position2D(), 0.05f);
        if (hitColliders)
        {
            Character character = hit.collider.GetComponentInParent<Character>();
            if (character is Character)
                character.GetKilled(hit.point - origin);
            gameObject.SetActive(false);
            return;
        }
    }

    void FixedUpdate()
    {
        if(hit)
        {
            Character character = hit.collider.GetComponentInParent<Character>();
            if (character is Character)
                character.GetKilled(hit.point - origin);
            gameObject.SetActive(false);
            return;
        }

        Vector2 nextFrame = transform.position2D() + direction * Time.deltaTime * speed;
        hit = Physics2D.Linecast(transform.position2D(), nextFrame, LayerMask.GetMask(Layers.Walls, Layers.Characters));
        if (hit)
        {
            transform.position = hit.point;
            return;
        }

        transform.position = nextFrame;
    }
}
