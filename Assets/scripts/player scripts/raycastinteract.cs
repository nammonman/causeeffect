using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class raycastinteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform playerCamera;
    [SerializeField] public GameObject promptBox;
    [SerializeField] public TextMeshProUGUI promptText;
    [SerializeField] public float rayDist = 10f;
    bool inDialogueBox;

    // Update is called once per frame
    void Update()
    {
        
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
            RaycastHit hitObjects;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObjects, rayDist)) //raycast
            {

                if (!inDialogueBox)
                {
                    // check for capsule
                    // TODO: change name check to tag check
                    if (hitObjects.collider.gameObject.name == "Capsule" )
                    {
                        promptBox.SetActive(true);
                        promptText.text = "[E] to talk";
                    }
                    else if (hitObjects.collider.gameObject.name != "Capsule")
                    {
                        promptBox.SetActive(false);
                        promptText.text = "";
                    }
                }

                // check dialogue text
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitObjects.collider.gameObject.TryGetComponent(out npcinteract npc))
                    {
                        talkWithNpc(npc);
                        Debug.Log(npc.name);
                    }
                    else
                    {
                        Debug.Log("asdasd");
                    }

                }
                
            }
            else
            {
                promptBox.SetActive(false);
                promptText.text = "";
            }

    }

    void talkWithNpc(npcinteract npc)
    {
        if (inDialogueBox)
        {
            dialoguecontroller.instance.SkipLine();
        }
        else
        {
            if (true) // TODO: check if npc tag
            {
                dialoguecontroller.instance.startDialogue(npc.dialogueObject, npc.StartAt, npc.name);
            }
        }
    }

    void JoinConversation()
    {
        inDialogueBox = true;
        playermovement.canMovePlayer = false;
        playermovement.canMoveCamera = false;
    }

    void LeaveConversation()
    {
        inDialogueBox = false;
        playermovement.canMovePlayer = true;
        playermovement.canMoveCamera = true;
    }

    private void OnEnable()
    {
        dialoguecontroller.OnDialogueStarted += JoinConversation;
        dialoguecontroller.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        dialoguecontroller.OnDialogueStarted -= JoinConversation;
        dialoguecontroller.OnDialogueEnded -= LeaveConversation;
    }
}
