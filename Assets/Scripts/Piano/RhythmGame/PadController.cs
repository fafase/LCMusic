﻿using UnityEngine;
using System.Collections;
using System;

public interface IPadController
{
	void HandleBpm();
	float BarBeat { get; }

	void CheckTimeForBeat(float elapsedTime);
	void ResetBeat();

	void SetEndPadCollisionEnter(IPadMovement padMovement);
	void SetEndPadCollisionExit(IPadMovement padMovement);
	void OnPointerEnter();
	void SetBPM(float value);
}

public class PadController : MonoBehaviour , IPadController
{
	private IRhythmController rhythmController = null;
	private IRhythmStreak rhythmStreak = null;

	public float BarBeat{ get; private set;}

	private Color selfColor = Color.black;

	private float [] bpms = null;
	private int currentIndex = 0;

	private ObjectPool pool = null;
	private Transform container = null;
	private GameObject prefab = null;

	private IPadMovement currentPadMovement = null;
	private int index = -1;
	private float bpm = 0f;

	public void Init(IRhythmController newRhythmController, IRhythmStreak newRhythmStreak,
		int newIndex, float [] newBpms, float newBarBeat, ObjectPool newPool, GameObject newPrefab, Transform newContainer)
	{
		this.rhythmController = newRhythmController;
		this.rhythmStreak = newRhythmStreak;
		this.index = newIndex;
		this.bpms = newBpms;
		this.BarBeat = newBarBeat;
		this.selfColor = this.gameObject.GetComponent<MeshRenderer> ().material.color;
		this.pool = newPool;
		this.prefab = newPrefab;
		this.container = newContainer;
	}

	public void HandleBpm ()
	{
		
	}

	public void CheckTimeForBeat(float elapsedTime)
	{
		if(currentIndex >= this.bpms.Length) { return; }
		if (elapsedTime >= this.bpms [this.currentIndex]) 
		{
			CreateBlock();
			if (++currentIndex >= this.bpms.Length) 
			{
				return;
			}
		}
	}
	public void ResetBeat()
	{
		currentIndex = 0;
	}

	private void CreateBlock()
	{
		GameObject obj = this.pool.PopFromPool(this.prefab, false, true, this.container);
		obj.transform.position = this.transform.position;
		IPadMovement padMovement = obj.GetComponent<IPadMovement>();
		padMovement.InitPadMovement(this as IPadController, this.selfColor, this.rhythmController.ContainerPad, this.bpm);
	}

	public void SetEndPadCollisionEnter(IPadMovement padMovement)
	{
		this.currentPadMovement = padMovement;
	}

	public void SetEndPadCollisionExit(IPadMovement padMovement)
	{
		this.currentPadMovement = null;
		this.rhythmStreak.ResetStreak();
	}

	public void OnPointerEnter()
	{
		if(this.currentPadMovement != null)
		{
			this.rhythmStreak.IncreaseStreak(this.index);
			this.currentPadMovement.TapOnPadSuccessful();
			this.currentPadMovement = null;
			return;
		}
		this.rhythmStreak.ResetStreak();
	}

	public void SetBPM(float value)
	{
		this.bpm = value;
	}
}
