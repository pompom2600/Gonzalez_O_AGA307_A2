using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();    
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
        
    }



}
