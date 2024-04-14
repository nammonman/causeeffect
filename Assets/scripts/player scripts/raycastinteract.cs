using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class raycastinteract : MonoBehaviour
{
    // Start is called before the first frame update
    public npcineract npcinteract;
    [SerializeField] public Transform playerCamera;

    void Start()
    {
        npcinteract = GetComponent<npcineract>();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (npcinteract != null)
        {
            Ray playerCameraRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hitObjects;

            if (!npcinteract.isDisplayingText && Physics.Raycast(playerCameraRay, out hitObjects) && Input.GetKeyDown(KeyCode.E) && hitObjects.collider.gameObject.name != "player")
            {
                Debug.Log("Raycast hit object: " + hitObjects.collider.gameObject.name);
                if (hitObjects.collider.gameObject.name == "Capsule")
                {
                    npcinteract.changeText();
                }
                //Debug.Log(npcinteract.isDisplayingText);
            }

            if (npcinteract.isDisplayingText && Input.GetKeyDown(KeyCode.E))
            {
                npcinteract.clearText();
                //Debug.Log(npcinteract.isDisplayingText);
            }

        }
        else
        {
            Debug.Log("sddfgafsgasdfgdsfg");
        }
    }
        
}
