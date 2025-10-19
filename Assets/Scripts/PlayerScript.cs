using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool isGrounded;
    public bool isRight;
    public Animator anim;
    public LayerMask groundLayer;
    public int health;
    public int lives;
    public GameObject weapon;
    public int moveDirection;
    HelperScript helper;

    void Start()
    {
        lives = 3;
        health = 3;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.GetMask("Ground");
        helper = gameObject.AddComponent<HelperScript>();
    }

    public bool ExtendedRayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.75f; // length of raycast
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

        float xvel, yvel;

        xvel = rb.linearVelocity.x;
        yvel = rb.linearVelocity.y;

        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            xvel = -2.75f;
            helper.DoFlipObject(true);
            ExtendedRayCollisionCheck(0.25f, 0);
            anim.SetBool("isRunning", true);
        }

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            xvel = 2.75f;
            helper.DoFlipObject(false);
            ExtendedRayCollisionCheck(-0.25f, 0);
            anim.SetBool("isRunning", true);
        }

        //do ground check

        if (ExtendedRayCollisionCheck(-0.25f, 0) == true)
        {
            isGrounded = true;
            if (yvel < 0)
            {
                anim.SetBool("isJumping", false);
            }
        }
        else
        {
            isGrounded = false;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            yvel = 5.65f;

            print("Jump");
            anim.SetBool("isJumping", true);
            anim.SetBool("isRunning", false);
        }

        if (xvel == 0)
        {
            anim.SetBool("isRunning", false);
        }

        rb.linearVelocity = new Vector3(xvel, yvel, 0);

        if (lives == 0)
        {
            SceneManager.LoadScene("SampleScene");
            lives = 3;
        }

        if (health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            lives = lives - 1;

        }

        Shoot();



    }

    public void Shoot()
    {
        moveDirection = 1;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            GameObject clone;
            clone = Instantiate(weapon, transform.position, transform.rotation);    
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();       
            rb.linearVelocity = transform.right * 15;  
            rb.transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z + 1);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health = health - 1;
        }

        if (other.gameObject.tag == "DeathPlain")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}