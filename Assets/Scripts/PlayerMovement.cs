using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;


    private bool jump;
    private bool crouch;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();    
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        jump = Input.GetButton("Jump");
        //Debug.Log(jump);
        crouch = Input.GetButton("Crouch");
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        
    }



}
