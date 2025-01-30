using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubstanceSlot : MonoBehaviour
{
    public GameObject jigsawPrefab;
    public Canvas canvas;
    public bool EmptySpawn;
    void OnEnable()
    {
        EmptySpawn = true;
        SpawnJigsawInCanvas();
    }
    public void SpawnJigsawInCanvas()
    {
        if (!EmptySpawn) return;

        GameObject spawnedJigsaw = Instantiate(jigsawPrefab);
        spawnedJigsaw.transform.SetParent(canvas.transform, false);
        RectTransform spawnedRectTransform = spawnedJigsaw.GetComponent<RectTransform>();
        RectTransform slotRectTransform = GetComponent<RectTransform>();

        if (spawnedRectTransform != null && slotRectTransform != null)
        {
            spawnedRectTransform.anchoredPosition = slotRectTransform.anchoredPosition;
            spawnedRectTransform.anchoredPosition += new Vector2(-40f, 0f);
            spawnedRectTransform.localScale *= 0.5f;
        }

        DragandDrop dragAndDrop = spawnedJigsaw.GetComponent<DragandDrop>();
        Jigsaw jigsaw = spawnedJigsaw.GetComponent<Jigsaw>();

        if (jigsaw != null)
        {
            jigsaw.spawnedBySlot = this;
        }

        if (dragAndDrop != null)
        {
            dragAndDrop.canvas = canvas;
            dragAndDrop.OnEndDragAction = () =>
            {
                SpawnJigsawInCanvas();
            };
        }
        EmptySpawn = false;
    }
}
