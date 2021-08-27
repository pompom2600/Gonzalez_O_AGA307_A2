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
    public Vector2 secondRoom;
    private Vector3 startingPos;
    private Torch torch;


    private SpriteRenderer spriteRenderer;
    private Color originalCol;



    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        startingPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalCol = spriteRenderer.color;
        torch = GetComponentInChildren<Torch>();
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

    public void HealthRegn() //Regenerate the health
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

    private void OnTriggerStay2D(Collider2D other) //To the Second Level
    {
        if (other.CompareTag("Door") && Input.GetKeyDown(KeyCode.W))
        {
            transform.position = secondRoom;
            //UIManager.instance.StartCoroutine("Transition");
            health = 4;
            torch.RechargeBattery();
            UIManager.instance.lvlText.text = ("Floor 2");
            startingPos = transform.position;
        }
    }

    public IEnumerator Spotted() //When Character is spotted
    {
        health = Mathf.Clamp(health - 1, 0, 4);
        UIManager.instance.UpdateHealth();
        Time.timeScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        spriteRenderer.color = Color.red;
       // UIManager.instance.crossFade;
        yield return StartCoroutine(WaitForRealSeconds(2f));

        transform.position = startingPos;
        Time.timeScale = 1;
        spriteRenderer.color = originalCol;
        controller.hasReset = false;
        torch.RechargeBattery();
        yield return null;
    }

}
