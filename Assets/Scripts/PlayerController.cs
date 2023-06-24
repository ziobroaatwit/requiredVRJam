using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;
    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private float groundDrag;
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatGround;
    private bool isGrounded;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    Vector3 moveDirection;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck();
        playerInput();
        speedClamp();

        //apply drag
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on ground movement
        if (isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    private void groundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatGround);
        //Debug.Log("IsGrounded: " + isGrounded);
    }
    private void playerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jumping
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }
    private void speedClamp()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if too high. (Could this be clamped?)
        if(flatVelocity.magnitude>moveSpeed)
        {
            Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y, limitedVel.z);
        }
    }
    private void jump()
    {
        //reset y velocity to always jump the exact same height.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    private void resetJump()
    {
        readyToJump = true;
    }
}
