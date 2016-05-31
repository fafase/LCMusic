﻿using System;
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
	public Warmup warmup = null;
	public string [] description = null;

	public Lesson (){}

	public void Clean()
	{
		this.name = null;
		this.rhythm.Clean();
		this.rhythm = null;
		this.warmup.Clean();
		this.warmup = null;
		for(int i = 0; i < this.description.Length; i++)
		{
			this.description[i] = null;
		}
		this.description = null;
	}
}

[Serializable]
public class Rhythm
{
	public Beat [] beat = null;
	public float bar = 4.0f;
	public string introText = null;
	public int streakChallenge = 0;
	public int bpmChallenge = 0;

	public string IntroText
	{ 
		get 
		{ 
			return String.Format(
				this.introText, 
				this.streakChallenge,
				this.bpmChallenge); 
		} 
	}

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

[Serializable]
public class Warmup
{
	public int [] keylist = null;

	public void Clean()
	{
		this.keylist = null;
	}
}

