using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Newspaper : MonoBehaviour
{
    public void readNewspaper()
    {
        gameObject.GetComponentInChildren<Canvas>().enabled = true;
    }
}
