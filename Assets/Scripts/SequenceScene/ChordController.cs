using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChordController : MonoBehaviour
{
    public EmptyString[] emptyStrings;
    public StringController[] stringCtrl;

    private void Start()
    {
        if (this.emptyStrings == null || this.emptyStrings.Length == 0)
        {
            Debug.LogError("[LCMusic] Missing EmptyString objects");
        }
        if (this.stringCtrl == null || this.stringCtrl.Length == 0)
        {
            Debug.LogError("[LCMusic] Missing stringCtrl objects");
        }
    }

    public void SetChord(ChordCreator creator)
    {
       /* IEnumerable<int> strings = creator.Strings;
        int k = 0;
        foreach (int i in strings)
        {
            switch (i)
            {
                case -1:
                    this.stringCtrl[k].ResetButtonFret();
                    this.emptyStrings[k].SetState(0);
                    break;
                case 0:
                    this.stringCtrl[k].ResetButtonFret();
                    this.emptyStrings[k].SetState(1);
                    break;
                default:
                    this.emptyStrings[k].SetState(0);
                    this.stringCtrl[k].SetButtonFret(i);
                    break;
            }
            k++;
        }*/
        // Get Each string and pass the value
    }
}
