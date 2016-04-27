using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChordCreator : MonoBehaviour
{
    private Chord chord = null;
    public IEnumerable<int> Strings { get { return this.chord.strings as IEnumerable<int>; } }

    public void Init(Chord newChord)
    {
        this.chord = newChord;
        Text text = this.gameObject.GetComponentInChildren<Text>();
        text.text = chord.name;
    }
}
