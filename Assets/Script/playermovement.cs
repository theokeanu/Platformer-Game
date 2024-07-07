using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour
{
    [SerializeField] public ParticleSystem dust;

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    private BoxCollider2D coll;
    float dirx = 0f;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask jumpfromground;
    [SerializeField] private AudioClip jumpingsound;
    private Vector3 respawnPoint;
    private enum MovementState {  idle, running, jump, fall};

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");

        rb2d.velocity = new Vector2(dirx * movementSpeed, rb2d.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded() || Input.GetKeyDown(KeyCode.W) && isgrounded()){
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            CreateDust();
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;
        if(dirx > 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        }
        else if (dirx < 0)
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb2d.velocity.y > .1f)
        {
            state = MovementState.jump;
        }
        else if(rb2d.velocity.y < -.1f)
        {
            state = MovementState.fall;
        }

        animator.SetInteger("state", (int)state);
    } 

    private bool isgrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpfromground);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }
        else if(collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
        }
    }
    void CreateDust()
    {
        dust.Play();
    }

    private void jumpsound()
    {
        SoundManager.instance.PlaySound(jumpingsound);
    }
}
