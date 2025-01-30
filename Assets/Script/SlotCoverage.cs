using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCoverage
{
    public int row;
    public int column;
    public float coverage;
    public SlotCoverage(int row, int column, float coverage)
    {
        this.row = row;
        this.column = column;
        this.coverage = coverage;
    }

}
