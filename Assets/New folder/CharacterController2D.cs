using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    //Player Jump, Left, Right
    public float jumpForce = 3f; //Jumpforce
    public float speed = 5f;
    public float gravity = 3f;
    public GameObject player;
    private Rigidbody2D rB2D;

    //Player Crouch
    [Range(0, 1)] public float crouchSpeed = .40f; // Crouch speed
    private Collider2D crouchDisableCollider; //Collider to disable when crouching
    public Transform ceilingCheck; //position marking to cehck for ceilings
    private float ceilingRadius = .2f; //Radius of the overlap circle to determine if player can stand up


    public Transform groundCheck; //position marking to check if grounded

    bool grounded;
    public LayerMask groundMask; //Mask for grounchecking
    private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded

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
        rB2D = GetComponent<Rigidbody2D>(); //the players rigidbody

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    void Fixedupdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[1].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch) //If crouching, check to see if the character can stand up
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

                move *= crouchSpeed; //Reduce the speed by the crouchSpeed multiplier

                if (crouchDisableCollider != null) //Disable one of the colliders when crouching
                    crouchDisableCollider.enabled = false;
            }

            else
            {
                if (crouchDisableCollider != null) //Enable collider when not crouching
                    crouchDisableCollider.enabled = true;

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }

            }

            Vector3 targetVelocity = new Vector2(move * 10f, rB2D.velocity.y); //Move character by finding the target velocity        

            if (move > 0 && !facingRight) //if the input is moving the player right and the player is facing left
                Flip(); //flip the player

            else if (move < 0 && facingRight)//if the input is moving the player left and the player is facing right
                Flip(); //flip the player

        }

        if (grounded && jump)
        {
            grounded = false;
            rB2D.AddForce(new Vector2(0f, jumpForce));
        }
    }
     private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
