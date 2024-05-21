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
    bool canInteract = true;

    public delegate void DialogueEnter(string GUID, bool E);
    public static event DialogueEnter OnDialogueEnter;

    public delegate void PlayerMoveable(bool b);
    public static event PlayerMoveable SetPlayerMoveable;

    public void Start()
    {
        canInteract = true;
    }
    // Update is called once per frame
    public void Update()
    {
        
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
        RaycastHit hitObject;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
        {
            if (canInteract)
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
                else if (!hitObject.collider.gameObject.CompareTag("npc"))
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
        SetCanInteract(false);
        SetPlayerMoveable?.Invoke(false);
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        SetCanInteract(true);
        SetPlayerMoveable?.Invoke(true);
        wholeDialogueContainer.SetActive(false);
    }

    public void SetCanInteract(bool b) 
    {
        canInteract = b; 
    }

    private void OnEnable()
    {
        DialogueParser.OnDialogueExit += LeaveConversation;
        InGamePauseMenu.SetPlayerMoveable += SetCanInteract;
    }

    private void OnDisable()
    {
        DialogueParser.OnDialogueExit -= LeaveConversation;
        InGamePauseMenu.SetPlayerMoveable += SetCanInteract;
    }
}
