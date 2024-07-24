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

    public delegate void DialogueEnter(string GUID, bool E);
    public static event DialogueEnter OnDialogueEnter;

    public void Start()
    {
        GameStateManager.canPlayerInteract = true;
    }
    // Update is called once per frame
    public void Update()
    {
        
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
        {
            if (GameStateManager.canPlayerInteract)
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
    private void EnablePausing()
    {
        StartCoroutine(PauseDelay(0.1f));
    }

    IEnumerator PauseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.canPause = true;
        //Debug.Log("Enabled Pausing");
    }
    public void JoinConversation()
    {
        GameStateManager.canPlayerInteract = false;
        GameStateManager.canPlayerMove = false;
        GameStateManager.canPlayerMoveCamera = false;
        GameStateManager.canPause = false;
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        GameStateManager.canPlayerInteract = true;
        GameStateManager.canPlayerMove = true;
        GameStateManager.canPlayerMoveCamera = true;
        EnablePausing();
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
