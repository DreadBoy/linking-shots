using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Pellet : MonoBehaviour {

    public Vector2 direction = Vector2.zero;
    public float speed = 20;

    TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.time /= speed;

        direction.Normalize();

        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position2D(), 0.5f);
        //if (hitColliders)
        //    Destroy(gameObject);
    }

    void FixedUpdate ()
    {

        Vector2 nextFrame = transform.position2D() + direction * Time.deltaTime * speed;

        if (Physics2D.Linecast(transform.position2D(), nextFrame))
        {
            Destroy(gameObject);
            return;
        }

        transform.position = nextFrame;
    }
}
