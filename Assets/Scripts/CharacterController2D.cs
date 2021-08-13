using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    //Player Jump, Left, Right
    public float jumpForce = 3f; //Jumpforce
    public float maxSpeed = 5f;
    public GameObject player;
    private Rigidbody2D rB2D;

    //Grounded
    public Transform groundCheck; //position marking to check if grounded
    public LayerMask groundMask; //Mask for grounchecking
    public float groundedRadius = .49f; // Radius of the overlap circle to determine if grounded
    bool grounded;

    //Player Crouch
    [Range(0, 1)] public float crouchSpeed = .40f; // Crouch maxSpeed
    private CapsuleCollider2D capsule; //CapsuleCollider
    public Transform ceilingCheck; //position marking to cehck for ceilings
    public float ceilingRadius = .2f; //Radius of the overlap circle to determine if player can stand up
    private Vector2 originalColliderSize;
    private Vector2 crouchColliderSize;
    private Vector2 crouchColliderOffset;
    private bool wasCrouching = false;
    private bool isCrouching = false;

    //Plantpot (evade)
    public bool isHiding = true;
    private bool isInView;
    private SpriteRenderer sR;
    [SerializeField] private Color hideColor;
    private Color defaultColor;

    //Flip
    private bool facingRight = true;


    private List<Vector2> collisionNormals = new List<Vector2>();

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider2D>();
        originalColliderSize = capsule.size;
        crouchColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
        crouchColliderOffset = new Vector2(0, - originalColliderSize.y / 4f);

        rB2D = GetComponent<Rigidbody2D>(); //the players rigidbody
        sR = GetComponent<SpriteRenderer>();// The players spriteRenderer 
        defaultColor = sR.color;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach(ContactPoint2D c in collision.contacts)
        {
            collisionNormals.Add(c.normal);
        }
    }

    void FixedUpdate()
    {
        rB2D.WakeUp();
        collisionNormals.Clear();
        bool wasGrounded = grounded;

        if (isHiding)
            sR.color = hideColor;

        else
        {
            sR.color = defaultColor;
            if (isInView)
            {
                sR.color = Color.red;
            }
        }

        isInView = false;
        grounded = false;
        isHiding = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch && wasCrouching) //If crouching & was crouching, check to see if the character can stand up
        {
            //stand

            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundMask)) //If the character has a ceiling preventing them from standing up, keep them crouching
            {
                crouch = true; //stay crouching
            }
        }

        if (!grounded)
        {
            foreach(Vector2 normal in collisionNormals) //Stop sticking to walls
            {
                if ((normal.x < 0 && move > 0) || (normal.x > 0 && move < 0))
                {
                    move = 0;
                }
            }
        }

        else
        {
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= crouchSpeed; //Reduce the maxSpeed by the crouchSpeed multiplier

                capsule.size = crouchColliderSize; // Halve the collider height
                capsule.offset = crouchColliderOffset; // Move the offset down by half the new hight
            }

            else
            {
                capsule.offset = Vector2.zero;  // Move offset up by half height
                capsule.size = originalColliderSize; // Double collider hight

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            isCrouching = crouch;
        }


        if (move > 0 && !facingRight) //if the input is moving the player right and the player is facing left
            Flip(); //flip the player

        else if (move < 0 && facingRight)//if the input is moving the player left and the player is facing right
            Flip(); //flip the player


        Vector2 targetVelocity = new Vector2(move * 10f, rB2D.velocity.y); //Move character by finding the target velocity        

        Vector2 forceVector = targetVelocity - rB2D.velocity; //Difference between input and current velocity
        if (forceVector.magnitude > maxSpeed) //Clamping the force to max speed
        {
            forceVector.Normalize();
            forceVector *= maxSpeed;
        }

        rB2D.AddForce(forceVector, ForceMode2D.Impulse); //Jumping

        if (grounded && jump)
        {
            grounded = false;
            rB2D.velocity = new Vector2(rB2D.velocity.x, jumpForce);
        }
    }

    //Player facing left or right------------------------------------------------------
     private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //Trigger things--------------------------------------------------------------------
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Evade") && isCrouching)
            isHiding = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player") && other.CompareTag("EnemyView") && isHiding == false)
        { 
            //isInView = true;
            PlayerMovement player = GetComponent<PlayerMovement>();
            player.StartCoroutine("Spotted");
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingRadius);
    }
}
