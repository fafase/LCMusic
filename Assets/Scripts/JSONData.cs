using System;
using System.Collections.Generic;

[Serializable]
public class RootObject : IDisposable
{
	public Lesson [] lesson = null;
	public RootObject()
	{
		
	}

	public void Dispose()
	{
		foreach (Lesson lesson in this.lesson) {
			lesson.Clean ();
		}
		this.lesson = null;
	}
}

[Serializable]
public class Lesson
{
	public string name = null;
	public bool available = false;
	public Rhythm [] rhythm = null;

	public Lesson (){}

	public void Clean()
	{
		this.name = null;
		foreach (Rhythm r in this.rhythm) {
			r.Clean ();
		}
	}
}

[Serializable]
public class Rhythm
{
	public float [] bpms = null;

	public Rhythm() { }

	public void Clean()
	{
		this.bpms = null;
	}
}

