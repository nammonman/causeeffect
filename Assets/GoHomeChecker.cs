using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnEnable()
    {
        if (GameStateManager.gameStates.currentTimeOfDay < 2)
        {
            List<string> list = gameObject.GetComponent<IObjProperties>().funcs;
            list.Clear();
            list.Add("Monologue_there is time left in the day. I should go back to the lab");
            
        }
        
    }

   
}
