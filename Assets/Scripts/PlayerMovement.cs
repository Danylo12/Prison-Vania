using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 MoveInput;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myCapsuleCollider2D;
    [SerializeField] float RunSpeed = 8f;
    [SerializeField] float JumpSpeed = 25f;
    [SerializeField] float ClimbSpeed = 6f;
    private float gravityScaleOnStart;
    


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleOnStart = myRigidbody.gravityScale;


    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();

    }

    void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
        Debug.Log(MoveInput);
    }

    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
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
        if (!myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
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
}
