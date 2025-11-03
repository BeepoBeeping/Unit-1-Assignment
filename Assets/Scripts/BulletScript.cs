using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public LayerMask groundLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    public bool ExtendedRayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.35f; // length of raycast
        bool hitSomething = false;

        // convert x and y offset into a Vector3 
        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        //cast a ray downward 
        RaycastHit2D hit;


        hit = Physics2D.Raycast(transform.position + offset, -Vector2.up, rayLength, groundLayer);

        Color hitColor = Color.white;


        if (hit.collider != null)
        {

            print("Player has collided with Ground layer");
            hitColor = Color.green;
            hitSomething = true;
        }
        // draw a debug ray to show ray position
        // You need to enable gizmos in the editor to see these
        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, hitColor);

        return hitSomething;

    }


    // Update is called once per frame
    void Update()
    {
        if (ExtendedRayCollisionCheck(0.5f, 0) == true)
        {
            Destroy(gameObject);
        }

        if(ExtendedRayCollisionCheck(-0.5f, 0) == true)
        {
            Destroy(gameObject);
        }
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
}
