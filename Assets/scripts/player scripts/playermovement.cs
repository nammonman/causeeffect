using UnityEngine;
using System.Collections;

public class playermovement : MonoBehaviour
{
    private Vector3 playerKeyboardInput;
    private Vector3 playerMouseInput;
    private float xRotation;
    private float yRotation;
    private float playerSpeed = 3;

    [SerializeField] private Rigidbody player;
    [SerializeField] private float playerNormalSpeed = 3;
    [SerializeField] private float playerSprintSpeed = 6;

    [SerializeField] private float playerJump;
    [Space]
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
        player.velocity = new Vector3(MoveVector.x, player.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space) && GameStateManager.gameStates.canPlayerJump )
        {
            player.AddForce(Vector3.up * playerJump, ForceMode.Impulse);
            GameStateManager.gameStates.canPlayerJump = false;
            StartCoroutine(EnableJumpAfterDelay(1.5f)); // Cooldown period
        }
    }

    IEnumerator EnableJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.gameStates.canPlayerJump = true;
    }
    private void MovePlayerCamera()
    {
        xRotation -= playerMouseInput.y * playerCameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, playerMouseInput.x * playerCameraSensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }


}
