using UnityEngine;
using System;
using System.Collections;

public interface ILine
{
	KeyNoteValue GetKeyNoteValue();
}
public sealed class LineController : MonoBehaviour, ILine 
{
	[SerializeField] private KeyNoteValue keyNote;

	private LineContainer line = null;
	private void Awake()
	{
		this.line = new LineContainer(this as ILine);
		if(this.keyNote.a == 0 && this.keyNote.b == 0){ throw new Exception("Value needed for keyNote");}
	}

	public KeyNoteValue GetKeyNoteValue()
	{
		return this.keyNote;
	}
}

[Serializable]
public class LineContainer
{
	private ILine line = null;
	public LineContainer(ILine line)
	{
		this.line = line;
	}
}

[Serializable]
public class KeyNoteValue
{
	public int a,b;
}
