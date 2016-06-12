using UnityEngine;
using System;
using System.Collections;

public interface IMusicalScore
{
	
}
public sealed class ScoreController : MonoBehaviour, IMusicalScore 
{
	private ScoreContainer score = null;

	private void Awake () 
	{
		this.score = new ScoreContainer(this as IMusicalScore);
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
