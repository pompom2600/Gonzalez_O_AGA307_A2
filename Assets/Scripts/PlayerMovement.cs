using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;

    public int health = 4;

    private bool jump;
    private bool crouch;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    private Vector3 startingPos;

    private SpriteRenderer spriteRenderer;
    private Color originalCol;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        startingPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalCol = spriteRenderer.color;
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        jump = Input.GetButton("Jump");
        crouch = Input.GetButton("Crouch");
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        
    }

    public void HealthRegn()
    {
        health = Mathf.Clamp(health + 1, 0, 4);
        UIManager.instance.UpdateHealth();
    }

    public IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    public IEnumerator Spotted()
    {
        health = Mathf.Clamp(health - 1, 0, 4);
        UIManager.instance.UpdateHealth();
        Time.timeScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        spriteRenderer.color = Color.red;
        yield return StartCoroutine(WaitForRealSeconds(2f));

        Time.timeScale = 1;
        transform.position = startingPos;
        spriteRenderer.color = originalCol;
        yield return null;
    }

}
