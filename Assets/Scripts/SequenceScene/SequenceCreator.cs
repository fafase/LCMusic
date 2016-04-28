using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LCHelper;

public class SequenceCreator : MonoBehaviour
{
    [SerializeField]
    private Text sequenceName = null;
    public GameObject [] chordButton;
	
	private void Start ()
    {
        if (this.sequenceName == null) { Debug.LogError("[LCMusic] Missing text"); }
        Sequence sequence = Save.DeserializeFromPlayerPrefs<Sequence>(AppController.CurrentData);

        if (sequence == null) { return; }
        this.sequenceName.text = sequence.name;
        SetChordNames(sequence.chords as IEnumerable<Chord>);
	}

    private void SetChordNames(IEnumerable<Chord>chords)
    {
        int i = 0;
        foreach (Chord chord in chords)
        {
            this.chordButton[i].GetComponent<ChordCreator>().Init(chord);
            i++;
        }
    }
}
