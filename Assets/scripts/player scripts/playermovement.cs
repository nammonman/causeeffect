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
        GameStateManager.gameStates.canPlayerJump = true;
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
}
