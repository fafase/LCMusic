using System;
using System.Collections.Generic;
[Serializable]
public class Chord : IDisposable
{
    public string name = null;
    public int[] strings = null;

    public void Dispose()
    {
        this.name = null;
        this.strings = null;
    }
}
[Serializable]
public class Sequence :IDisposable
{
    public string name = null;
    public Chord [] chords = null;

    public void Dispose()
    {
        this.name = null;
        for (int i = 0; i < this.chords.Length; i++)
        {
            this.chords[i] = null;
        }
        this.chords = null;
    }
}
[Serializable]
public class RootObject : IDisposable
{
    public Sequence[] sequences = null;

    public void Dispose()
    {
        for (int i = 0; i < this.sequences.Length; i++)
        {
            this.sequences[i] = null;
        }
    }
}
