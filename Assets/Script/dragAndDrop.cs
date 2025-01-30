using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragandDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private chemTableSlot gridManager;
    private float currentRotation;
    private bool isDragging = false;
    private Jigsaw jigsaw;
    public System.Action OnEndDragAction;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        gridManager = FindObjectOfType<chemTableSlot>();
        jigsaw = GetComponent<Jigsaw>();
        currentRotation = rectTransform.localEulerAngles.z;
    }

    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.R))
        {
            RotateJigsaw();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
        rectTransform.localScale = Vector3.one * 1.0f;

        if (gridManager != null && jigsaw != null)
        {
            gridManager.ClearPreviousPosition(jigsaw);
        }
        //ยกขึ้นแล้ว set EmptySpawn เป็น true 
        if (jigsaw.spawnedBySlot != null)
        {
            Debug.Log($"Jigsaw lifted from slot: {jigsaw.spawnedBySlot.name}");
            if (!jigsaw.alreadyAttach)
            {
                jigsaw.spawnedBySlot.EmptySpawn = true;
            }
        }
        else
        {
            Debug.Log("Jigsaw is not spawned by any slot.");
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("stopDrag");

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        OnEndDragAction?.Invoke();

        if (IsOverlapGrid())
        {
            Debug.Log("Within Grid");
            if (gridManager != null && jigsaw != null)
            {
                gridManager.ProcessDrop(rectTransform, jigsaw, canvas);
            }
        }
        else
        {
            Debug.Log("Outside Grid");
            jigsaw.alreadyAttach = false;
            Destroy(gameObject);
        }
        gridManager.CalculateStats();
    }

    private bool IsOverlapGrid()
    {
        Rect gridBounds = gridManager.GetScreenSpaceRect(gridManager.gridParent, canvas);
        Rect jigsawBounds = gridManager.GetScreenSpaceRect(rectTransform, canvas);
        if (gridBounds.Overlaps(jigsawBounds)) // ตรวจสอบว่าทับซ้อนหรือไม่
        {
            // คำนวณพื้นที่ซ้อนทับ
            Rect intersection = gridManager.RectIntersection(gridBounds, jigsawBounds);
            float overlapArea = intersection.width * intersection.height;

            // คำนวณพื้นที่ทั้งหมดของ jigsaw
            float jigsawArea = jigsawBounds.width * jigsawBounds.height;

            // ตรวจสอบว่า Overlap มากกว่า 50% หรือไม่
            float overlapPercentage = (overlapArea / jigsawArea) * 100f;
            Debug.Log($"Overlap Percentage: {overlapPercentage}%");

            if (overlapPercentage >= 50f)
            {
                Debug.Log("Overlap is sufficient (>= 50%)");
                return true;
            }
            else
            {
                Debug.Log("Overlap is insufficient (< 50%)");
            }
        }
        else
        {
            Debug.Log("No overlap with grid.");
        }

        return false;
    }

    private void RotateJigsaw()
    {
        currentRotation -= 90f;
        if (currentRotation < 360f)
        {
            currentRotation += 360f;
        }
        rectTransform.localEulerAngles = new Vector3(0, 0, currentRotation);

        List<List<int>> rotatedShape = new List<List<int>>();
        for (int column = 0; column < jigsaw.Width; column++)
        {
            List<int> newRow = new List<int>();
            for (int row = jigsaw.Height - 1; row >= 0; row--)
            {
                newRow.Add(jigsaw.shape[row][column]);
            }
            rotatedShape.Add(newRow);

        }
        jigsaw.shape = rotatedShape;
        Debug.Log("Rotated Shape:");
        foreach (var row in rotatedShape)
        {
            Debug.Log(string.Join(", ", row));
        }
    }
}