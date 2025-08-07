using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 MoveInput;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider2D;
    private BoxCollider2D myFeetCollider2D;
    [SerializeField] float RunSpeed = 7f;
    [SerializeField] float JumpSpeed = 22f;
    [SerializeField] float ClimbSpeed = 5f;
    private float gravityScaleOnStart;

    private bool isAlive = true;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] AudioClip deathSFX;
    
    
    
    


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleOnStart = myRigidbody.gravityScale;


    }

    void Update()
    {
        if (!isAlive) {return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();

    }

    void OnFire(InputValue value)
    {
        if (!isAlive) {return; }

        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return; }
        MoveInput = value.Get<Vector2>();
        Debug.Log(MoveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {return; }
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, JumpSpeed);
        }
        
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(MoveInput.x * RunSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
        
        
    }

    void ClimbLadder()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleOnStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        myRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, MoveInput.y * ClimbSpeed);
        myRigidbody.velocity = climbVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            FindObjectOfType<CameraShaking>().Shake();
            StartCoroutine(DelayedReload());
        }
    }
    
    IEnumerator DelayedReload()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        yield return new WaitForSeconds(0.6f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }


}

