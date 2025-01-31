using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw : MonoBehaviour
{
    public SubstanceSlot spawnedBySlot;
    public string shapeInput;
    public bool alreadyAttach = false;
    [SerializeField] public List<int> values = new List<int>(new int[4]);
    public List<List<int>> shape = new List<List<int>>();
    public int Height => shape.Count;
    public int Width => shape.Count > 0 ? shape[0].Count : 0;
    private Jigsaw jigsaw;
    private chemTableSlot gridManager;
    private void Start()
    {
        jigsaw = GetComponent<Jigsaw>();
        gridManager = FindObjectOfType<chemTableSlot>();
        shape.Clear();
        string[] rows = shapeInput.Split('/');
        foreach (string row in rows)
        {
            List<int> rowList = new List<int>();
            foreach (char c in row)
            {
                if (int.TryParse(c.ToString(), out int value))
                {
                    rowList.Add(value);
                }
            }
            shape.Add(rowList);
        }
    }
    private void Update()
    {
        if (IsMouseOver())
        {
            if (gameObject.GetComponent<CanvasGroup>().blocksRaycasts == true)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0.8f;
            }
            
            if (Input.GetMouseButtonDown(1) && alreadyAttach)
            {
                gridManager.ClearPreviousPosition(jigsaw);
                Debug.Log("clear position jigsaw");
                jigsaw.alreadyAttach = false;
                Destroy(gameObject);
                Debug.Log("destroy jigsaw");
                gridManager.CalculateStats();
                Debug.Log("recalulate sum");
            }
        }
        else if (gameObject.GetComponent<CanvasGroup>().alpha < 1)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    private bool IsMouseOver()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
    }
}