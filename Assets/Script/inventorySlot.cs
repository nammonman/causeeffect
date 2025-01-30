using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class inventorySlot : MonoBehaviour
{
    public int row;
    public int column;
    public RectTransform slotRect;
    private chemTableSlot gridManager;
    private Canvas canvas;

    private void Awake()
    {
        slotRect = GetComponent<RectTransform>();
        gridManager = GetComponentInParent<chemTableSlot>();
        canvas = GetComponentInParent<Canvas>();
    }
    // public void OnDrop(PointerEventData eventData)
    // {
    //     if (eventData.pointerDrag != null)
    //     {
    //         RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
    //         Jigsaw jigsaw = eventData.pointerDrag.GetComponent<Jigsaw>();

    //         if (draggedRect != null && jigsaw != null)
    //         {
    //             gridManager.ProcessDrop(draggedRect, jigsaw, canvas);
    //         }
    //     }
    // }
}
