using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jigsaw : MonoBehaviour
{
    public SubstanceSlot spawnedBySlot;
    public string shapeInput;
    public bool alreadyAttach = false;
    [SerializeField] public List<int> values = new List<int>(new int[4]);
    public List<List<int>> shape = new List<List<int>>();
    public int Height => shape.Count;
    public int Width => shape.Count > 0 ? shape[0].Count : 0;
    private void Start()
    {
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
}
