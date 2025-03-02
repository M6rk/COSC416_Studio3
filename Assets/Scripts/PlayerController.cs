using UnityEngine;
using System.Collections; // for IEnumerator (for dash)

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpHeight = 5f;
    public float dashSpeed = 20f;
    private bool isDashing = false;
    private int jumpCount = 0; // keep track of # jumps for double jump
    private Rigidbody rb;
    private bool isGrounded;
    public Transform cameraTransform; // reference for freelook camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        // lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // handle movement (WASD)
        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }

        handleMove(inputVector);

        // handle jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handleJump(jumpHeight);
        }
        // handle dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            //StartCouroutine allows for the delay in dash to take place (i.e., with an IENumerator method)
            StartCoroutine(Dash());
        }
        rotatePlayer();
    }

    public void handleMove(Vector2 inputVector)
    {
        // get camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // flatten camera directions
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // calculate movement
        Vector3 movement = (cameraForward * inputVector.y + cameraRight * inputVector.x) * moveSpeed * Time.deltaTime;

        // use Space.World to ensure the player's movement works with the cinemachine freelook camera
        transform.Translate(movement, Space.World);
    }
    // allow jumping if player is on ground (if collision detected)
    public void handleJump(float jumpHeight)
    {
        if (isGrounded || jumpCount < 2)
        {
            if (jumpCount == 1) // when player has already jumped and is jumping again (i.e., double jump)
            {
                Debug.Log("Double Jump");
            }
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
            jumpCount++;
        }
    }
    // use ground tag with plane object & box objects to detect if player can jump
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // reset jump count when player lands
        }
    }
    // rotate player prefab to face camera direction
    private void rotatePlayer()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Keep the player upright
        if (cameraForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = targetRotation; // Instantly set the rotation to the target rotation
        }
    }
    // dash method -- IENumerator used to allow for dash to last for 0.2f (i.e., a specified duration)
    private IEnumerator Dash()
    {
        isDashing = true;

        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);
        Debug.Log("Dash");
        yield return new WaitForSeconds(0.2f);

        //reset values after dash
        rb.linearVelocity = Vector3.zero;
        isDashing = false;
    }
}