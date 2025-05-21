using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Set animation based on movement
        animator.SetBool("isMoving", moveDirection != Vector3.zero);
    }

    private void FixedUpdate()
    {
        // Move the player using velocity
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y; // Preserve Y velocity (e.g. gravity/jump)
        rb.linearVelocity = velocity;
    }

    public void MoveForward()
    {
        RotatePlayer(180f);
        moveDirection = transform.forward;
    }

    public void MoveBackward()
    {
        RotatePlayer(0f);
        moveDirection = transform.forward;
    }

    public void MoveLeft()
    {
        RotatePlayer(270f);
        moveDirection = transform.forward;
    }

    public void MoveRight()
    {
        RotatePlayer(90f);
        moveDirection = transform.forward;
    }
    public void Idle()
    {
        moveDirection = Vector3.zero;
    }

    public void JumpForward()
    {
        if (isGrounded)
        {
            // Apply jump force upward and forward
            Vector3 jumpDir = transform.forward + Vector3.up;
            rb.AddForce(jumpDir.normalized * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void RotatePlayer(float yAngle)
    {
        transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Simple ground check
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
