using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject playerPrefab;
    public TextMeshProUGUI dropdownText;
    public GameObject pauseMenu;

    public static event Action OnStart;
    public static event Action OnEnd;

    private Dictionary<string, Vector3> scenePositionLookup = new Dictionary<string, Vector3>
    {
        { "Home", new Vector3(-49, 3, -15) },
        { "HomePersonalRoom", new Vector3(-5, 1, 7) },
        { "Home true ending", new Vector3(-18.48f, 3, -7.86f) },
        { "Home good ending", new Vector3(-26, 3, -1) },
        { "WorkHallway", new Vector3(-2, 7, 61) },      
        { "WorkLab", new Vector3(-1, 7, -11) },
        { "WorkPersonalRoom", new Vector3(0, 1.5f, -8) },
        { "WorkPresidentRoom", new Vector3(3, 1.5f, 1) },
        { "presentation", new Vector3(18, 2, -8) },
        { "presentation tutorial", new Vector3(18, 2, -8) },
        { "presentation empty", new Vector3(16, 3, 5) },
        { "TestScene", new Vector3(0, 0, 0) },
        { "MainMenu", new Vector3(0, 2, 0) },
    };

    private Dictionary<string, string> defaultCameraLookup = new Dictionary<string, string>
    {
        { "Home", "CameraFarTerminal" },
        { "HomePersonalRoom", "deskcam" },
        { "Home true ending", "CameraFarTerminal" },
        { "Home good ending", "Main Camera" },
        { "WorkHallway", "Main Camera" },
        { "WorkLab", "Main Camera" },
        { "WorkPersonalRoom", "Main Camera" },
        { "WorkPresidentRoom", "Main Camera" },
        { "presentation", "Main Camera" },
        { "presentation tutorial", "Main Camera" },
        { "presentation empty", "Main Camera" },
        { "TestScene", "Main Camera" },
        { "MainMenu", "Main Camera" },
    };


    public void LoadSceneDebug()
    {
        LoadSceneByName(dropdownText.text);
    }

    private void OnEnable()
    {
        GameStateManager.OnLoadNewScene += LoadSceneByName;
        GameStateManager.OnLoadNewSceneWithPos += LoadSceneByNameAndPos;
    }

    private void OnDisable()
    {
        GameStateManager.OnLoadNewScene -= LoadSceneByName;
        GameStateManager.OnLoadNewSceneWithPos -= LoadSceneByNameAndPos;
    }

    // Method to load a scene by name and teleport player
    public void LoadSceneByName(string sceneName)
    {
        // Check if the scene name exists in the dictionary
        if (scenePositionLookup.ContainsKey(sceneName))
        {
            // Load the scene asynchronously
            StartCoroutine(LoadSceneAndSetPlayerPos(sceneName));
        }
        else
        {
            Debug.LogError("Scene not found in the lookup table: " + sceneName);
        }
    }

    public void LoadSceneByNameAndPos(string sceneName, Vector3 pos)
    {
        // Check if the scene name exists in the dictionary
        if (scenePositionLookup.ContainsKey(sceneName))
        {
            // Load the scene asynchronously
            StartCoroutine(LoadSceneAndSetPlayerPos(sceneName, pos));
        }
        else
        {
            Debug.LogError("Scene not found in the lookup table: " + sceneName);
        }
    }

    private IEnumerator<AsyncOperation> LoadSceneAndSetPlayerPos(string sceneName, Vector3? pos = null)
    {
        OnStart?.Invoke();
        // Load the new scene asynchronously
        ShowSecretText.currentSceneTexts.Clear();
        
        GameObject player = GameObject.FindGameObjectWithTag("player prefab");
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        GameStateManager.setPausedState(true);
        GameStateManager.gameStates.canPause = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene has fully loaded
        while (!asyncLoad.isDone)
        {
            GameStateManager.setPausedState(true);
            GameStateManager.gameStates.canPause = false;

            yield return asyncLoad;
        }
        
        if (sceneName != "MainMenu")
        {
            GameStateManager.setPausedState(pauseMenu.activeSelf);
            GameStateManager.gameStates.canPause = true;
        }
        else
        {
            GameStateManager.setPausedState(true);
            GameStateManager.gameStates.canPause = false;
            pauseMenu.SetActive(false);
        }

        // Teleport the player to the specific position after scene loads
        if (playerPrefab != null && pos == null)
        {
            rb.position = scenePositionLookup[sceneName];
            Debug.Log($"Player teleported to {scenePositionLookup[sceneName]} in {sceneName}");
        }
        else if (playerPrefab != null && pos != null) 
        {
            rb.position = pos.Value;
            Debug.Log($"Player teleported to {pos.Value} in {sceneName}");
        }
        else
        {
            Debug.Log(sceneName + " is invalid");
        }
        
        SelectCamera(defaultCameraLookup[sceneName]);
        GameStateManager.setSceneName(sceneName);
        rb.isKinematic = false;

        var temp = GameObject.FindGameObjectsWithTag("secret text");
        foreach (var item in temp)
        {
            ShowSecretText.currentSceneTexts.Add(item);
        }
        GameStateManager.setSecretText(GameStateManager.gameStates.canSeeSecretText);
        GameStateManager.SetUpdateFlags();
        OnEnd?.Invoke();
        if (sceneName != "MainMenu")
        {
            InGamePauseMenu.SetCursorVisible(false);
        }
        else
        {
            InGamePauseMenu.SetCursorVisible(true);
        }
    }

    public void SelectCamera(string n)
    {
        // Loop through all active scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.isLoaded)
            {
                // Get all root game objects in the current scene
                GameObject[] rootObjects = scene.GetRootGameObjects();
                List<GameObject> players = new List<GameObject>();

                // List to hold all cameras
                Camera[] allCameras = new Camera[0];

                // Loop through all root objects and find cameras, including in children
                foreach (GameObject rootObject in rootObjects)
                {
                    Camera[] camerasInObject = rootObject.GetComponentsInChildren<Camera>(true);
                    // Append found cameras to the allCameras array
                    Camera[] newCamerasArray = new Camera[allCameras.Length + camerasInObject.Length];
                    allCameras.CopyTo(newCamerasArray, 0);
                    camerasInObject.CopyTo(newCamerasArray, allCameras.Length);
                    allCameras = newCamerasArray;
                }

                // Loop through all found cameras
                foreach (Camera cam in allCameras)
                {
                    if (cam.gameObject.name == n)
                    {
                        // Enable the camera that matches the given name
                        cam.gameObject.SetActive(true);
                    }
                    else
                    {
                        // Disable all other cameras
                        cam.gameObject.SetActive(false);
                    }
                }

            }
        }
    }
}
