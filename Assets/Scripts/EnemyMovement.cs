using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Patrol, Idle }

public class EnemyMovement : MonoBehaviour
{
    private EnemyState currentState = EnemyState.Patrol;

    public Transform targetA;
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
        switch (currentState)
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

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

   /* IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, targetA.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetA.position, Time.deltaTime * baseSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        Flip();

        while (Vector3.Distance(transform.position, targetB) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetB, Time.deltaTime * baseSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        Flip();

        StartCoroutine(Move());
    }*/
}
