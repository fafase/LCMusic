using System;
using System.Collections.Generic;
using UnityEngine;

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
	public Note [] note = null;

	public void Clean()
	{
		foreach(Note n in note)
		{
			n.Clean();
		}
		this.note = null;
	}
}

[Serializable]
public class Note
{
	public int key;
	public float length;
	public string color = null;

	public Color KeyColor 
	{ 
		get
		{ 
			string[] strings = this.color.Split(","[0] );
			Color color = new Color(1,0,0,1);
			for (int i = 0; i < strings.Length; i++) 
			{
				try
				{
					color[i] = System.Single.Parse(strings[i]);
				}catch(Exception)
				{
					color[i] = 1f;
				}
			}
			return color;
		} 
	}
	
	public Note(){}
	public void Clean()
	{
		this.color = null;
	}
}

