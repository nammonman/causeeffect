using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Subtegral.SceneGraphSystem.Editor;

public class SceneObjectFinder : MonoBehaviour
{
    public static GameObject FindObjectInScene(string sceneName, string objectName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
        {
            Debug.LogWarning("scene " + sceneName + " is not open");
            return null;
        }

        GameObject[] rootObjects = scene.GetRootGameObjects();
        Queue<GameObject> queue = new Queue<GameObject>();

        foreach (GameObject root in rootObjects)
            queue.Enqueue(root);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            if (current.name == objectName)
                return current;

            foreach (Transform child in current.transform)
                queue.Enqueue(child.gameObject);
        }
        
        Debug.LogWarning("cannot find GameObject with name: " + objectName);
        return null;

    }

    public static GameObject[] FindObjectsInSceneWithTag(string sceneName, string tag)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        List<GameObject> foundObjects = new List<GameObject>();

        if (!scene.isLoaded)
        {
            Debug.LogWarning("scene " + sceneName + " is not open");
            return null;
        }


        GameObject[] rootObjects = scene.GetRootGameObjects();
        Queue<GameObject> queue = new Queue<GameObject>();

        foreach (GameObject root in rootObjects)
            queue.Enqueue(root);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            if (current.tag == tag)
                foundObjects.Add(current);

            foreach (Transform child in current.transform)
                queue.Enqueue(child.gameObject);
        }

        if (foundObjects.Count > 0)
        {
            return foundObjects.ToArray();
        }
        else
        {
            Debug.LogWarning("cannot find GameObject with tag: " + tag);
            return null;
        }
        
    }

    private static GameObject BFSFind(GameObject[] rootObjects, string objectName)
    {
        Queue<GameObject> queue = new Queue<GameObject>();

        foreach (GameObject root in rootObjects)
            queue.Enqueue(root);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            if (current.name == objectName)
                return current;

            foreach (Transform child in current.transform)
                queue.Enqueue(child.gameObject);
        }
        return null;
    }
}
