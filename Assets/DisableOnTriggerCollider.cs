using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnTriggerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the GameObject entering the trigger has the tag "player prefab"
        if (other.CompareTag("Player"))
        {
            // Disable this GameObject
            gameObject.SetActive(false);

            // Alternatively, disable just this script
            // this.enabled = false;
        }
    }
}
