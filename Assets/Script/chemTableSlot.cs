using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class chemTableSlot : MonoBehaviour
{
    public static List<List<int>> chemTable = new List<List<int>>();
    private Dictionary<Jigsaw, SlotCoverage> previousPositions = new Dictionary<Jigsaw, SlotCoverage>();
    public RectTransform gridParent;
    public List<int> sum_value = new List<int>() { 0, 0, 0, 0 };
    public List<Image> displayObjects; // UI Images สำหรับแสดงผล (8 ตัว)
    public List<GameObject> negativeObjects; // GameObjects สำหรับแสดงผลค่าลบ (4 ตัว)
    public Sprite[] numberSprites; // Sprite สำหรับตัวเลข 0-9
    public int specialCount = 0;
    private bool pause = true;
    private void Awake()
    {
        if (gridParent == null)
        {
            gridParent = GetComponent<RectTransform>();
        }

        if (chemTable.Count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < 8; j++)
                {
                    row.Add(0);
                }
                chemTable.Add(row);
            }
        }

        foreach (var negativeObject in negativeObjects)
        {
            if (negativeObject != null)
            {
                negativeObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (pause)
        {
            GameStateManager.setPausedState(true);
        }
        
    }

    public void PrintChemTable()
    {
        Debug.Log("Current ChemTable:");

        for (int rowIndex = 0; rowIndex < chemTable.Count; rowIndex++)
        {
            string rowValues = string.Join(", ", chemTable[rowIndex]);
            Debug.Log($"Row {rowIndex + 1}: {rowValues}"); // เพิ่ม Row Index
        }
    }
    public void ProcessDrop(RectTransform jigsawRect, Jigsaw jigsaw, Canvas canvas)
    {
        Debug.Log("in the drop process");
        List<SlotCoverage> coverageList = new List<SlotCoverage>();
        foreach (Transform slotTransform in gridParent)
        {
            inventorySlot slot = slotTransform.GetComponent<inventorySlot>();
            if (slot != null)
            {
                Rect jigsawBounds = GetScreenSpaceRect(jigsawRect, canvas);
                Rect slotBounds = GetScreenSpaceRect(slot.slotRect, canvas);

                if (jigsawBounds.Overlaps(slotBounds)) // ตรวจสอบการครอบคลุม
                {
                    Rect intersection = RectIntersection(jigsawBounds, slotBounds);
                    float coverageArea = intersection.width * intersection.height;
                    coverageList.Add(new SlotCoverage(slot.row, slot.column, coverageArea));
                }
            }
        }

        SlotCoverage bestSlot = FindBestSlot(coverageList, jigsaw);
        if (bestSlot != null)
        {
            if (SnapJigsawToSlot(jigsawRect, bestSlot, jigsaw))
            {
                UpdateChemTable(bestSlot, jigsaw);
                previousPositions[jigsaw] = bestSlot;
                PrintChemTable();
            }
        }
        else
        {
            //กรณีหาช่องที่เหมาะไม่ได้
            if (jigsaw.spawnedBySlot != null && !jigsaw.alreadyAttach)
            {
                jigsaw.spawnedBySlot.EmptySpawn = true;
                jigsaw.spawnedBySlot.SpawnJigsawInCanvas();
            }
            Debug.Log("No suitable slot found for this Jigsaw.");
            jigsaw.alreadyAttach = false;
            Destroy(jigsaw.gameObject);
        }
    }

    // ค้นหา Slot ที่มีพื้นที่ครอบคลุมมากที่สุด
    private SlotCoverage FindBestSlot(List<SlotCoverage> coverageList, Jigsaw jigsaw)
    {
        SlotCoverage bestSlot = null;
        float maxTotalCoverage = 0;

        foreach (var coverage in coverageList)
        {
            // คำนวณความครอบคลุมรวมของทุก Slot ที่ Jigsaw ครอบคลุม
            float totalCoverage = 0;
            bool isValid = true;

            for (int row = coverage.row; row < coverage.row + jigsaw.Height; row++)
            {
                for (int column = coverage.column; column < coverage.column + jigsaw.Width; column++)
                {
                    // ค้นหา Slot ที่ตรงกับตำแหน่ง (row, column)
                    SlotCoverage slotCoverage = coverageList.Find(c => c.row == row && c.column == column);

                    if (slotCoverage != null)
                    {
                        totalCoverage += slotCoverage.coverage;
                    }
                    else
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid)
                    break;
            }

            if (isValid && totalCoverage > maxTotalCoverage)
            {
                maxTotalCoverage = totalCoverage;
                bestSlot = coverage;

                // เพิ่ม Debug Log เพื่อแสดงค่าของ Slot นี้
                Debug.Log($"New Best Slot Found: Row {bestSlot.row}, Column {bestSlot.column}, Coverage Area: {bestSlot.coverage}, Total Coverage: {totalCoverage}");
            }
        }
        if (bestSlot != null)
        {
            Debug.Log($"Final Best Slot: Row {bestSlot.row}, Column {bestSlot.column}, Total Coverage: {maxTotalCoverage}");
        }
        else
        {
            Debug.Log("No suitable slot found.");
        }

        return bestSlot;
    }

    private bool SnapJigsawToSlot(RectTransform jigsawRect, SlotCoverage bestSlot, Jigsaw jigsaw)
    {
        // คำนวณตำแหน่งกึ่งกลางของ Slot หลายช่องที่ Jigsaw ครอบคลุม
        Vector3 averageWorldPosition = Vector3.zero;
        int slotCount = 0;

        for (int row = bestSlot.row; row < bestSlot.row + jigsaw.Height; row++)
        {
            for (int column = bestSlot.column; column < bestSlot.column + jigsaw.Width; column++)
            {
                if (jigsaw.shape[row - bestSlot.row][column - bestSlot.column] == 1 && (row >= chemTable.Count || column >= chemTable[row].Count || chemTable[row][column] == 1))
                {
                    Debug.Log($"Cannot place Jigsaw: chemTable[{row}][{column}] is occupied.");
                    jigsaw.alreadyAttach = false;
                    Destroy(jigsawRect.gameObject);
                    CalculateStats();
                    return false;
                }

                foreach (Transform slotTransform in gridParent)
                {
                    inventorySlot slot = slotTransform.GetComponent<inventorySlot>();
                    if (slot != null && slot.row == row && slot.column == column)
                    {
                        // รวมตำแหน่ง World Space ของทุก Slot ที่ Jigsaw ครอบคลุม
                        averageWorldPosition += slot.slotRect.transform.position;
                        slotCount++;
                    }
                }
            }
        }

        if (slotCount > 0)
        {
            // คำนวณตำแหน่งเฉลี่ยใน World Space
            averageWorldPosition /= slotCount;
            // แปลงตำแหน่งเฉลี่ยจาก World Space เป็น Local Space ของ Canvas
            Vector3 localPositionInCanvas = jigsawRect.parent.InverseTransformPoint(averageWorldPosition);
            // อัปเดตตำแหน่ง Jigsaw ใน Local Space
            jigsawRect.localPosition = localPositionInCanvas;
            Debug.Log($"Jigsaw snapped to Slots: Row {bestSlot.row}-{bestSlot.row + jigsaw.Height - 1}, " +
                      $"Column {bestSlot.column}-{bestSlot.column + jigsaw.Width - 1}");
        }
        jigsaw.alreadyAttach = true;
        CalculateStats();
        return true;
    }

    private void UpdateChemTable(SlotCoverage bestSlot, Jigsaw jigsaw)
    {
        for (int i = 0; i < jigsaw.Height; i++)
        {
            for (int j = 0; j < jigsaw.Width; j++)
            {
                if (jigsaw.shape[i][j] == 1)
                {
                    int row = bestSlot.row + i;
                    int column = bestSlot.column + j;

                    if (row < chemTable.Count && column < chemTable[row].Count)
                    {
                        chemTable[row][column] = 1;
                        Debug.Log($"Updated slot {row},{column} to 1");
                    }
                }
            }
        }
    }

    public void ClearPreviousPosition(Jigsaw jigsaw)
    {
        if (previousPositions.ContainsKey(jigsaw))
        {
            SlotCoverage previousSlot = previousPositions[jigsaw];

            for (int i = 0; i < jigsaw.Height; i++)
            {
                for (int j = 0; j < jigsaw.Width; j++)
                {
                    int row = previousSlot.row + i;
                    int column = previousSlot.column + j;
                    if (jigsaw.shape[i][j] == 1)
                    {
                        if (row < chemTable.Count && column < chemTable[row].Count)
                        {
                            chemTable[row][column] = 0;
                            Debug.Log($"Cleared slot {row},{column} to 0");
                        }
                    }
                }
            }
            previousPositions.Remove(jigsaw);
        }
    }
    // Utility: คำนวณขอบเขตใน World Space
    public Rect GetScreenSpaceRect(RectTransform rectTransform, Canvas canvas)
    {
        Vector2 size = RectTransformUtility.PixelAdjustRect(rectTransform, canvas).size;
        Vector2 position = (Vector2)rectTransform.position - (size * 0.5f);

        return new Rect(position, size);
    }

    // Utility: คำนวณพื้นที่ทับซ้อนของ Rect
    public Rect RectIntersection(Rect a, Rect b)
    {
        float xMin = Mathf.Max(a.xMin, b.xMin);
        float xMax = Mathf.Min(a.xMax, b.xMax);
        float yMin = Mathf.Max(a.yMin, b.yMin);
        float yMax = Mathf.Min(a.yMax, b.yMax);

        if (xMax >= xMin && yMax >= yMin)
        {
            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }
        return Rect.zero;
    }
    public void CalculateStats()
    {
        for (int i = 0; i < sum_value.Count; i++)
        {
            sum_value[i] = 0;
        }
        Jigsaw[] allJigsaws = FindObjectsOfType<Jigsaw>();
        int count = 0;
        specialCount = 0;

        foreach (Jigsaw jigsaw in allJigsaws)
        {
            if (jigsaw.alreadyAttach)
            {
                count++;
                for (int i = 0; i < sum_value.Count; i++)
                {
                    if (i < jigsaw.values.Count)
                    {
                        sum_value[i] += jigsaw.values[i];
                    }
                }
                if (jigsaw.name == "jigsaw_special(Clone)")
                {
                    specialCount++;
                }
            }
        }
        Debug.Log("special count: " + specialCount);
        Debug.Log($"Updated sum_value: {string.Join(", ", sum_value)} and jigsaw on table are {count}");
        UpdateStats();
    }
    private void UpdateStats()
    {
        if (displayObjects.Count != 8 || numberSprites.Length < 10)
        {
            Debug.LogError("updateStatsError");
            return;
        }

        for (int i = 0; i < sum_value.Count; i++)
        {
            int value = sum_value[i];
            int tens;
            int ones;
            if (value < 0)
            {
                negativeObjects[i].SetActive(true);
            }
            else
            {
                negativeObjects[i].SetActive(false);
            }

            if (value > 99 || value < -99)
            {
                tens = 9;
                ones = 9;
            }
            else
            {
                tens = Mathf.Abs(value / 10);
                ones = Mathf.Abs(value % 10);
            }

            Image tensObject = displayObjects[i * 2];
            Image onesObject = displayObjects[i * 2 + 1];
            if (value < 10 && value >= 0)
            {
                tensObject.sprite = numberSprites[0];
            }
            else
            {
                tensObject.sprite = numberSprites[tens];
            }
            onesObject.sprite = numberSprites[ones];
        }
        Debug.Log("Stats updated successfully.");
    }

    // สรุปผลตอนกด confirm
    public void sumOutput()
    {
        if (specialCount == 8 && sum_value[0] < 8 && sum_value[1] < 48 && sum_value[2] > 18 && sum_value[3] > 28)
        {
            AddGlobalFlag("MIX_TimeBomb");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_I have made \"TimeBomb\"" });
            GameStateManager.setNewTLTitle("TIMEBOMB GET");
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[0] > 30 && sum_value[1] > 30 && sum_value[2] > 30 && sum_value[3] > 30 && GameStateManager.gameStates.globalFlags.Contains("RECIPE_AlienInvasion"))
        {
            AddGlobalFlag("MIX_AlienInvasion");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_I have made \"Alien Invasion\"" });
            GameStateManager.setNewTLTitle("ALIEN INVASION GET");
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[1] == 10 && sum_value[2] == -40)
        {
            AddGlobalFlag("MIX_StabilizerI");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_I have made \"Stabilizer I\"" });
            GameStateManager.setNewTLTitle("STABILIZER I GET");
            if (GameStateManager.gameStates.globalFlags.Contains("MIX_StabilizerII"))
            {
                AddGlobalFlag("RECIPE_AlienInvasion");
            }
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[0] == 25 && sum_value[3] == 27)
        {
            AddGlobalFlag("MIX_StabilizerII");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_I have made \"Stabilizer II\"" });
            GameStateManager.setNewTLTitle("STABILIZER II GET");
            if (GameStateManager.gameStates.globalFlags.Contains("MIX_StabilizerI"))
            {
                AddGlobalFlag("RECIPE_AlienInvasion");
            }
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[0] == 22 && sum_value[1] == -6 && sum_value[2] == 20 && sum_value[3] == 32)
        {
            AddGlobalFlag("MIX_EnhancedVision");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_I have made \"Enhanced Vision\"" });
            GameStateManager.setNewTLTitle("ENHANCED VISION GET");
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[0] > 30 && sum_value[3] > 10)
        {
            AddGlobalFlag("MIX_Overheat");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_OH NO! EVERYTHING IS BURNING!|Glitch_4", "CauseeffectText_dangerous mix", "LoadTL_2" });
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[0] > 19 && sum_value[2] > 11)
        {
            AddGlobalFlag("MIX_Explosive");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_it's sparking and popping... it's getting exponentialy more violent... WHY IS IT NOT STOPPING!?|Glitch_4", "CauseeffectText_dangerous mix", "LoadTL_2" });
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[2] == -20 && sum_value[1] < 0)
        {
            AddGlobalFlag("MIX_Corrosion");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_why is it corroding the instruments? that should not happen... is that blood!?|Glitch_4", "CauseeffectText_dangerous mix", "LoadTL_2" });
            StartCoroutine(UnloadCurrentScene());
        }
        else if (sum_value[3] < -20)
        {
            AddGlobalFlag("MIX_RadiationPoisoning");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_is that sound from the the Geiger counter?... ARGH IT HURTS!|Glitch_4", "CauseeffectText_dangerous mix", "LoadTL_2" });
            StartCoroutine(UnloadCurrentScene());
        }
        else
        {
            AddGlobalFlag("MIX_Nothing");
            TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_this mixture doesn't do anything..." });
            StartCoroutine(UnloadCurrentScene());
        }
        if (GameStateManager.gameStates.currentDay != 2)
        {
            GameStateManager.setIncrementTime();
        }
        pause = false;
        GameStateManager.setPausedState(false);
        
    }

    private void AddGlobalFlag(string flag)
    {
        if (!GameStateManager.gameStates.globalFlags.Contains(flag))
        {
            GameStateManager.gameStates.globalFlags.Add(flag);
        }
    }


    public void resetTable()
    {
        Jigsaw[] allJigsaws = FindObjectsOfType<Jigsaw>();
        foreach (Jigsaw jigsaw in allJigsaws)
        {
            if (jigsaw.alreadyAttach)
            {
                jigsaw.alreadyAttach = false;
                Destroy(jigsaw.gameObject);
            }
        }
        for (int i = 0; i < chemTable.Count; i++)
        {
            for (int j = 0; j < chemTable[i].Count; j++)
            {
                chemTable[i][j] = 0; // ตั้งค่าใหม่เป็น 0
            }
        }
        CalculateStats();
    }

    private IEnumerator UnloadCurrentScene(float seconds = 4)
    {
        yield return new WaitForSeconds(seconds);
        yield return null;
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}