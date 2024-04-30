using Subtegral.DialogueSystem.Runtime;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class raycastinteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform playerCamera;
    [SerializeField] public GameObject wholeDialogueContainer;
    [SerializeField] public TextMeshProUGUI promptText;
    [SerializeField] public float rayDist = 10f;
    bool inDialogueBox;

    public delegate void DialogueEnter(string GUID, bool E);
    public static event DialogueEnter OnDialogueEnter;
    //public static event Action OnDialogueEnded;
    // Update is called once per frame
    public void Update()
    {
        
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
        RaycastHit hitObject;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
        {
            if (!inDialogueBox)
            {
                if (hitObject.collider.gameObject.tag == "npc")
                {
                    promptText.text = "[E] to talk";
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log(hitObject.transform.name);
                    JoinConversation();
                    OnDialogueEnter?.Invoke(hitObject.transform.name, true);
                }
                else if (hitObject.collider.gameObject.tag != "npc")
                {
                    promptText.text = "";
                }
            }
        }
        else
        {
            promptText.text = "";
        }

    }


    public void JoinConversation()
    {
        inDialogueBox = true;
        playermovement.canMovePlayer = false;
        playermovement.canMoveCamera = false;
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        inDialogueBox = false;
        playermovement.canMovePlayer = true;
        playermovement.canMoveCamera = true;
        wholeDialogueContainer.SetActive(false);
    }

    private void OnEnable()
    {
        //OnDialogueStarted += JoinConversation;
        //OnDialogueEnded += LeaveConversation;
        DialogueParser.OnDialogueExit += LeaveConversation;
    }

    private void OnDisable()
    {
        //OnDialogueStarted -= JoinConversation;
        //OnDialogueEnded -= LeaveConversation;
        DialogueParser.OnDialogueExit -= LeaveConversation;
    }
}
