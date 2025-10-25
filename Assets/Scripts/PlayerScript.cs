using TMPro;
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
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;
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

        

        healthText.text = health.ToString();
        livesText.text = lives.ToString(); // ui
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

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) // movement
        {
            xvel = 2.75f;
            helper.DoFlipObject(false);
            ExtendedRayCollisionCheck(-0.25f, 0);
            anim.SetBool("isRunning", true);
        }

        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            xvel = -4f;
            helper.DoFlipObject(true);
            ExtendedRayCollisionCheck(0.25f, 0);
            anim.SetBool("isRunning", true);
        }

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift)) // sprint
        {
            xvel = 4f;
            helper.DoFlipObject(false);
            ExtendedRayCollisionCheck(-0.25f, 0);
            anim.SetBool("isRunning", true);
        }

        if (Input.GetKey("r"))
        {
            print("Game Reset");
            SceneManager.LoadScene("LVL1"); // reset game
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
            yvel = 5.75f;

            print("Jump");
            anim.SetBool("isJumping", true);
        }

        if (xvel == 0)
        {
            anim.SetBool("isRunning", false);
        }

        rb.linearVelocity = new Vector3(xvel, yvel, 0);

        if (lives == 0)
        {
            SceneManager.LoadScene("LVL1");
            lives = 3;
        }

        if (health == 0)
        {
            lives = lives - 1;
            transform.position = new Vector3(0f, 0f, 0f);
        }

        Shoot();



    }

    public void Shoot() // projectile 
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

    private void OnCollisionEnter2D(Collision2D other) // basic collision stuff lol
    {
        if (other.gameObject.tag == "Enemy")
        {
            health = health - 1;
        }

        if (other.gameObject.tag == "DeathPlain")
        {
            transform.position = new Vector3(0f, 0f, 0f);

            lives = lives - 1;
        }

        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}