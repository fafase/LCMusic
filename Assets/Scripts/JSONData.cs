using System;
using System.Collections.Generic;

[Serializable]
public class RootObject : IDisposable
{
	public Lesson [] lesson = null;
	public RootObject() { }

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
	public Rhythm rhythm = null;

	public Lesson (){}

	public void Clean()
	{
		this.name = null;
		this.rhythm.Clean();
		this.rhythm = null;
	}
}

[Serializable]
public class Rhythm
{
	public Beat [] beat = null;
	public float bar = 4.0f;
	public string introText = null;
	public int [] streakChallenge = null;
	public int [] bpmChallenge = null;

	public Rhythm() { }

	public void Clean()
	{
		this.beat = null;
		foreach(Beat b in beat)
		{
			b.Clean();
		}
		this.beat = null;
		this.introText = null;
		this.bpmChallenge = null;
		this.streakChallenge = null;
	}
}

[Serializable]
public class Beat
{
	public float [] bpms = null;
	public void Clean()
	{
		this.bpms = null;
	}
}

