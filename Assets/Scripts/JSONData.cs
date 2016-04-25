using System;
using System.Collections.Generic;
[Serializable]
public class Chord
{
    public string name = null;
    public int [] strings = null;
}
[Serializable]
public class Sequence
{
    public string name = null;
    public Chord [] chords = null;
}
[Serializable]
public class RootObject
{
    public Sequence[] sequences = null;
}
