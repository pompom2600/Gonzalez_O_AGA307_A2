using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Patrol, Idle }

public class EnemyMovement : MonoBehaviour
{
    private EnemyState currentState = EnemyState.Patrol;

    public Transform targetA; //Points A & B
    public Transform targetB;

    private Transform currentTarget;


    public float idleTime = 3f;
    private float timer = 0f;

    [SerializeField] private float moveSpeed = 3f;

    private CharacterController2D cc;
    private bool facingRight = true;



    private void Awake()
    {
        cc = GetComponent <CharacterController2D>();
        currentTarget = targetA;
    }

    private void FixedUpdate()
    {
        switch (currentState) //Enemy State
        {
            case EnemyState.Patrol:
                float targetDir = currentTarget.position.x - transform.position.x;
                float moveDir = targetDir > 0 ? 1 : -1;
                transform.localScale = new Vector3(moveDir, 1, 1);

                cc.Move(moveDir * moveSpeed * Time.fixedDeltaTime, false, false);

                if (Math.Abs(targetDir) <= 0.1f)//Transition to Idle state
                {
                    timer = idleTime;
                    currentState = EnemyState.Idle;
                }
                break;
            case EnemyState.Idle:
                timer -= Time.fixedDeltaTime;
                if (timer <= 0) //Transition to Patrol state
                {
                    currentTarget = (currentTarget == targetA) ? targetB : targetA;
                    currentState = EnemyState.Patrol;
                }
                break;

        }
    }

    private void Flip() //Fliping the image
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
