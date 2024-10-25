using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
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

    public delegate void DialogueEnter(string GUID, bool E, Dialogue D);
    public static event DialogueEnter OnDialogueEnter;

    private bool objInteractable = false;

    public void Start()
    {
        promptText.text = "[E] to interact";
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
        RaycastHit hitObject;
        if (GameStateManager.gameStates.canPlayerInteract && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
        {
            // check if tag is interactable
            if (hitObject.collider.gameObject.tag == "Iobj" || hitObject.collider.gameObject.tag == "npc")
            {
                objInteractable = true;
            }
            else
            {
                objInteractable = false;
            }

            // set prompt text
            if (objInteractable && !promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(true);
            }
            else if (!objInteractable && promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(false);
            }

            // get input and run funcs
            if (objInteractable && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(hitObject.transform.name);

                if (hitObject.collider.gameObject.tag == "npc")
                {
                    JoinConversation();
                    NpcDialogue npcDialogue = hitObject.collider.gameObject.GetComponent<NpcDialogue>();
                    if (npcDialogue != null)
                    {
                        if (npcDialogue.isFirstInteract)
                        {
                            OnDialogueEnter?.Invoke(npcDialogue.firstStartNode, true, npcDialogue.firstDialogue);
                        }
                        else
                        {
                            OnDialogueEnter?.Invoke(npcDialogue.secondStartNode, true, npcDialogue.secondDialogue);
                        }
                    }   
                }
                else if (hitObject.collider.gameObject.tag == "Iobj")
                {
                    
                }


            }

            
        }
        else if (promptText.gameObject.activeSelf)
        {
            objInteractable = false;
            promptText.gameObject.SetActive(false);
        }

    }

    IEnumerator PauseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.setPausedState(false);
        //Debug.Log("Enabled Pausing");
    }
    public void JoinConversation()
    {
        GameStateManager.setPausedState(true);
        GameStateManager.gameStates.isInDialogue = true;
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        GameStateManager.gameStates.isInDialogue = false;
        StartCoroutine(PauseDelay(0.1f));
        wholeDialogueContainer.SetActive(false);
    }

    private void OnEnable()
    {
        DialogueParser.OnDialogueExit += LeaveConversation;
    }

    private void OnDisable()
    {
        DialogueParser.OnDialogueExit -= LeaveConversation;
    }
}
