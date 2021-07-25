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

    //Player Crouch
    [Range(0, 1)] public float crouchSpeed = .40f; // Crouch maxSpeed
    private CapsuleCollider2D capsule; //CapsuleCollider
    public Transform ceilingCheck; //position marking to cehck for ceilings
    public float ceilingRadius = .2f; //Radius of the overlap circle to determine if player can stand up
    private Vector2 originalColliderSize;
    private Vector2 crouchColliderSize;
    private Vector2 crouchColliderOffset;

    public Transform groundCheck; //position marking to check if grounded

    bool grounded;
    public LayerMask groundMask; //Mask for grounchecking
    public float groundedRadius = .49f; // Radius of the overlap circle to determine if grounded

    private bool wasCrouching = false;
    private bool facingRight = true;

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

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

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

        if (grounded)
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

          

            if (move > 0 && !facingRight) //if the input is moving the player right and the player is facing left
                Flip(); //flip the player

            else if (move < 0 && facingRight)//if the input is moving the player left and the player is facing right
                Flip(); //flip the player

        }

        Vector2 targetVelocity = new Vector2(move * 10f, rB2D.velocity.y); //Move character by finding the target velocity        

        Vector2 forceVector = targetVelocity - rB2D.velocity; // Difference between input and current velocity
        if (forceVector.magnitude > maxSpeed) // Clamping the force to max speed
        {
            forceVector.Normalize();
            forceVector *= maxSpeed;
        }

        rB2D.AddForce(forceVector, ForceMode2D.Impulse);

        if (grounded && jump)
        {
            grounded = false;
            rB2D.velocity = new Vector2(rB2D.velocity.x, jumpForce);
        }
    }
     private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingRadius);
    }

}
