using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAcrossScenes : MonoBehaviour
{
    void Awake()
    {
        // Prevent this object from being destroyed when changing scenes
        DontDestroyOnLoad(gameObject);
    }
}
