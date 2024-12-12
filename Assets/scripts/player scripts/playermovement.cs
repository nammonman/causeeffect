using UnityEngine;
using System.Collections;

public class playermovement : MonoBehaviour
{
    private Vector3 playerKeyboardInput;
    private Vector3 playerMouseInput;
    public static float xRotation;
    public static float yRotation;
    private float playerSpeed = 3;

    [SerializeField] private Rigidbody player;
    [SerializeField] private float playerNormalSpeed = 3;
    [SerializeField] private float playerSprintSpeed = 6;

    [SerializeField] private float playerJump;
    [Space]
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float playerCameraSensitivity;

    private void OnEnable()
    {
        GameStateManager.OnFreezePlayer += FreezePlayer;
    }

    private void OnDisable()
    {
        GameStateManager.OnFreezePlayer -= FreezePlayer;
    }
    private void Update()
    {
        
        if (GameStateManager.gameStates.canPlayerMove)
        {
            playerKeyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            MovePlayer();
        }

        if (GameStateManager.gameStates.canPlayerMoveCamera)
        {
            playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MovePlayerCamera();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = playerSprintSpeed;
        }
        else
        {
            playerSpeed = playerNormalSpeed;
        }

        if (GameStateManager.gameStates.isPaused)
        {
            DeceleratePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(playerKeyboardInput) * playerSpeed;
        Quaternion targetRotation = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0);
        MoveVector = targetRotation * MoveVector;
        player.velocity = new Vector3(MoveVector.x, player.velocity.y, MoveVector.z);


        if (Input.GetKeyDown(KeyCode.Space) && GameStateManager.gameStates.canPlayerJump )
        {
            player.AddForce(Vector3.up * playerJump, ForceMode.Impulse);
            GameStateManager.gameStates.canPlayerJump = false;
            StartCoroutine(EnableJumpAfterDelay(1.2f)); // Cooldown period
        }
    }

    IEnumerator EnableJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!GameStateManager.gameStates.isPaused)
        {
            GameStateManager.gameStates.canPlayerJump = true;
        }
        
    }
    private void MovePlayerCamera()
    {
        // Accumulate and clamp the X rotation (vertical rotation)
        xRotation -= playerMouseInput.y * playerCameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Accumulate the Y rotation (horizontal rotation)
        yRotation += playerMouseInput.x * playerCameraSensitivity;

        // Apply both X and Y rotations 
        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void FreezePlayer(bool b)
    {
        player.isKinematic = b;
    }

    public void DeceleratePlayer()
    {
        // Get the current horizontal velocity (X, Z) and preserve the vertical velocity (Y)
        Vector3 horizontalVelocity = new Vector3(player.velocity.x, 0, player.velocity.z);

        if (horizontalVelocity.magnitude > 0.1f) // Only decelerate if there's significant horizontal movement
        {
            // Calculate horizontal deceleration
            Vector3 deceleration = -horizontalVelocity.normalized * 5 * horizontalVelocity.magnitude * Time.deltaTime;

            // Ensure we don’t reverse the horizontal direction
            if (deceleration.magnitude > horizontalVelocity.magnitude)
                horizontalVelocity = Vector3.zero;
            else
                horizontalVelocity += deceleration;

            // Set the updated velocity, preserving the vertical component
            player.velocity = new Vector3(horizontalVelocity.x, player.velocity.y, horizontalVelocity.z);
        }
        else
        {
            // Snap horizontal velocity to zero when close enough, preserving vertical component
            player.velocity = new Vector3(0, player.velocity.y, 0);
        }
    }
}
