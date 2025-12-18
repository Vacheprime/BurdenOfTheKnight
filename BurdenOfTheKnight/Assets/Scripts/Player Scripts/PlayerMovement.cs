using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float speedMultiplier = 1f;

    float horizontalInput;
    float verticalInput;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Increase speed
            speedMultiplier = 1.5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Reset
            speedMultiplier = 1f;
        }
    }

    private void MovePlayer()
    {
        // Preserve gravity / jumping 
        float yVelocity = rb.linearVelocity.y;

        // Build input vector (magnitude = stick amount)
        Vector3 input = new Vector3(horizontalInput, 0f, verticalInput);

        // Clamp diagonal magnitude
        input = Vector3.ClampMagnitude(input, 1f);

        if (input.sqrMagnitude == 0f)
        {
            // Immediate stop
            rb.linearVelocity = new Vector3(0f, yVelocity, 0f);
            return;
        }

        // Direction-relative movement (player forward/right)
        Vector3 moveDir =
            transform.forward * input.z +
            transform.right * input.x;

        // Constant speed scaled by joystick magnitude
        Vector3 velocity =
            moveDir.normalized * (moveSpeed * speedMultiplier * input.magnitude);

        rb.linearVelocity = new Vector3(
            velocity.x,
            yVelocity,
            velocity.z
        );
    }
}
