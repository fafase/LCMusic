using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class SequenceCreator : MonoBehaviour
{
    [SerializeField]
    private Text sequenceName = null;
    public GameObject [] chords;
	
	private void Start ()
    {
        if (this.sequenceName == null) { Debug.LogError("[LCMusic] Missing text"); }

        string str = PlayerPrefs.GetString(AppController.CurrentData, null);
        if (str == null) { return; }
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(Convert.FromBase64String(str));
        Sequence sequence =  bf.Deserialize(ms) as Sequence;
        if (sequence == null) { return; }
        this.sequenceName.text = sequence.name;
        SetChordNames(sequence.chords as IEnumerable<Chord>);
	}

    private void SetChordNames(IEnumerable<Chord>chords)
    {
        int i = 0;
        foreach (Chord chord in chords)
        {
            Text text = this.chords[i].GetComponentInChildren<Text>();
            text.text = chord.name;
            i++;
        }
    }
}
