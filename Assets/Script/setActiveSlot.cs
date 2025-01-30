using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setActiveSlot : MonoBehaviour
{
    public List<GameObject> slotSet1;
    public List<GameObject> slotSet2;
    public List<GameObject> slotSet3;
    public static bool haveSChem; // สำหรับรับค่าว่ามีสาร S จิง
    private int nowSet;

    private void Awake()
    {
        if (GameStateManager.gameStates.globalFlags.Contains("PCSolved"))
        {
            haveSChem = true;
        }
        nowSet = 1;
        ToggleSlotSet();
        forwardSet();
        backwardSet();
    }

    private void ToggleSlotSet()
    {
        CheckAndDestroyUnattachedJigsaws();
        if (nowSet == 1)
        {
            SetActiveState(slotSet1, true);
            SetActiveState(slotSet2, false);
            SetActiveState(slotSet3, false);
        }
        else if (nowSet == 2)
        {
            SetActiveState(slotSet1, false);
            SetActiveState(slotSet2, true);
            SetActiveState(slotSet3, false);
        }
        else if (nowSet == 3)
        {
            SetActiveState(slotSet1, false);
            SetActiveState(slotSet2, false);
            SetActiveState(slotSet3, true);
        }
    }

    private void SetActiveState(List<GameObject> slotSet, bool state)
    {
        foreach (var slot in slotSet)
        {
            if (slot.name == "slot_8" && !haveSChem)
            {
                slot.SetActive(false);
                continue;
            }
            if (slot != null)
            {
                slot.SetActive(state);
                SubstanceSlot substanceSlot = slot.GetComponent<SubstanceSlot>();
                if (substanceSlot != null)
                {
                    substanceSlot.enabled = state;
                }
            }
        }
    }

    public void forwardSet()
    {
        nowSet++;
        if (nowSet > 3)
        {
            nowSet = 1;
        }
        else if (nowSet < 1)
        {
            nowSet = 3;
        }

        ToggleSlotSet();
    }

    public void backwardSet()
    {
        nowSet--;
        if (nowSet > 3)
        {
            nowSet = 1;
        }
        else if (nowSet < 1)
        {
            nowSet = 3;
        }
        ToggleSlotSet();
    }

    private void CheckAndDestroyUnattachedJigsaws()
    {
        Jigsaw[] allJigsaws = FindObjectsOfType<Jigsaw>();

        foreach (var jigsaw in allJigsaws)
        {
            if (!jigsaw.alreadyAttach)
            {
                Destroy(jigsaw.gameObject);
            }
        }
    }
}
