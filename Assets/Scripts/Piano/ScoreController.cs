using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IMusicalScore
{
	void InitWithLesson(Lesson currentLesson);
}

public sealed class ScoreController : MonoBehaviour, IMusicalScore 
{
	[SerializeField] private GameObject notePrefab = null;
	private BarController [] bars = null;
	private ScoreContainer score = null;

	private void Awake () 
	{
		if(this.notePrefab == null){ throw new NullReferenceException("Missing Note prefab object"); }
		this.bars = this.gameObject.GetComponentsInChildren<BarController>();
		if(this.bars.Length != 4){ throw new NullReferenceException("Missing bar object"); }
		this.score = new ScoreContainer(this as IMusicalScore);
	}

	public void InitWithLesson(Lesson currentLesson)
	{
		if(currentLesson == null){ throw new Exception("Empty lesson"); }
		CreateNoteOnLine(currentLesson.warmup.note as IEnumerable<Note>);
	}

	private void CreateNoteOnLine(IEnumerable<Note>notes)
	{
		foreach(BarController bc in this.bars)
		{
			bc.CreateNoteWithPrefab(this.notePrefab, notes);  
		}	
	}
}

[Serializable]
public class ScoreContainer
{
	private IMusicalScore score = null;

	private float barWidth = 0.0f;
	public float BarWidth { get { return this.barWidth; } }

	public ScoreContainer(IMusicalScore score)
	{
		this.score = score;
		SetBarWidth((float)Screen.width);
	}

	private void SetBarWidth(float screenWidth)
	{
		this.barWidth = screenWidth / 4f;
	}

}
