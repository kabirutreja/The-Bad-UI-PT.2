using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tetrisgoal : MonoBehaviour
{
   
    public DialogueManager dm;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (tetrisblock.linesCleared >= 6)
        {
            dm.tetrisdone();
        }
                if (Input.GetKeyDown(KeyCode.Escape))
        {
            dm.tetrisclose();
        }
    }
}
