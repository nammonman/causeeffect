using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using System;
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
    public static event Action OnPCEnter;
    public static event Action OnMixEnter;
    private bool objInteractable = false;
    
    public void Start()
    {
        promptText.text = "[E] to interact";
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameStateManager.gameStates.canPlayerInteract)
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
            RaycastHit hitObject;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
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

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log(hitObject.transform.name);
                }

                // get input and run funcs
                if (objInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    

                    if (hitObject.collider.gameObject.tag == "npc")
                    {
                        JoinConversation();
                        NpcDialogue npcDialogue = hitObject.collider.gameObject.GetComponent<NpcDialogue>();
                        if (npcDialogue != null && npcDialogue.canInteract == true)
                        {
                            if (npcDialogue.isFirstInteract && npcDialogue.firstDialogue != null)
                            {
                                OnDialogueEnter?.Invoke(npcDialogue.firstDialogue.NodeLinks[0].TargetNodeGUID, true, npcDialogue.firstDialogue);
                                npcDialogue.isFirstInteract = false;    
                            }
                            else if (npcDialogue.secondDialogue != null)
                            {
                                OnDialogueEnter?.Invoke(npcDialogue.secondDialogue.NodeLinks[0].TargetNodeGUID, true, npcDialogue.secondDialogue);
                            }
                        }
                    }
                    else if (hitObject.collider.gameObject.tag == "Iobj")
                    {
                        
                        if (hitObject.collider.gameObject.name == "PC TRIGGER")
                        {
                            OnPCEnter?.Invoke();
                            InGamePauseMenu.SetCursorVisible(true);
                        }
                        else if (hitObject.collider.gameObject.name == "control panel interact")
                        {
                            if (GameStateManager.gameStates.currentTimeOfDay < 2)
                            {
                                OnMixEnter?.Invoke();
                                TriggerRunner.RunFuncsCaller(hitObject.collider.gameObject.GetComponent<IObjProperties>().funcs);
                            }
                            else
                            {
                                GameStateManager.setMonologue("I have already spent the whole day on this, I should go");
                            }
                            
                        }
                        else if (hitObject.collider.gameObject.name.StartsWith("FoundItem"))
                        {
                            TriggerRunner.RunFuncsCaller(hitObject.collider.gameObject.GetComponent<IObjProperties>().funcs);
                            Destroy(hitObject.collider.gameObject);
                        }
                        else if (hitObject.collider.gameObject.name.StartsWith("NEWSPAPER"))
                        {
                            hitObject.collider.gameObject.GetComponent<Newspaper>().readNewspaper();
                            InGamePauseMenu.SetCursorVisible(true);
                        }
                        else
                        {
                            TriggerRunner.RunFuncsCaller(hitObject.collider.gameObject.GetComponent<IObjProperties>().funcs);
                        }
                    }
                }

                /*if (GameStateManager.gameStates.canSeeSecretText && GameStateManager.gameStates.canReadSecretText && hitObject.collider.gameObject.tag == "secret text")
                {
                    string seenSecretText = hitObject.collider.gameObject.GetComponent<TMP_Text>().text;
                    int index = NotebookSwitcher.notes.IndexOf(seenSecretText);
                    if (index > -1 && !NotebookSwitcher.unlockedNotes.Contains(index))
                    {
                        NotebookSwitcher.unlockedNotes.Add(index);
                        StartCoroutine(SwitchPromptextForSeconds(2, "new entry added to notes"));
                        Debug.Log("new entry added to notes");
                        Debug.Log(seenSecretText);
                    }
                    
                }*/

            }
            else if (promptText.gameObject.activeSelf)
            {
                objInteractable = false;
                promptText.gameObject.SetActive(false);
            }
        }
        else
        {
            objInteractable = false;
            promptText.gameObject.SetActive(false);
        }

    }

    IEnumerator SwitchPromptextForSeconds(float delay, string s)
    {
        promptText.text = s;
        promptText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        promptText.gameObject.SetActive(false);
        promptText.text = "[E] to interact";
    }

    IEnumerator PauseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.setPausedState(false);
        //Debug.Log("Enabled Pausing");
    }

    public void JoinConversation()
    {
        InGamePauseMenu.SetCursorVisible(true);
        GameStateManager.setPausedState(true);
        GameStateManager.gameStates.canPause = false;
        GameStateManager.gameStates.isInDialogue = true;
        GameStateManager.gameStates.canPlayerInteract = false;
        promptText.gameObject.SetActive(false);
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        
        StartCoroutine(PauseDelay(0.1f));
        wholeDialogueContainer.SetActive(false);
        GameStateManager.gameStates.canPause = true;
        GameStateManager.gameStates.isInDialogue = false;
        GameStateManager.gameStates.canPlayerInteract = true;
        InGamePauseMenu.SetCursorVisible(false);
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
