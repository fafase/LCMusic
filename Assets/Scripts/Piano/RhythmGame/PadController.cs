using UnityEngine;
using System.Collections;
using System;

public interface IPadController
{
	void HandleBpm();
	float BarBeat { get; }

	void CheckTimeForBeat(float elapsedTime);
	void ResetBeat();
}

public class PadController : MonoBehaviour , IPadController
{
	[SerializeField] private GameObject padPrefab = null;

	private PadContainer padContainer = null;
	private IRhythmController rhytmController = null;
	 
	private float barBeat = 4f;
	public float BarBeat{ get { return this.barBeat; } }

	private Color selfColor = Color.black;

	private float [] bpms = null;
	private int currentIndex = 0;

	private float timer = 0f;
	private float count = 0f;

	private void Awake()
	{
		this.padContainer = new PadContainer (this as IPadController);
	}
		
	public void Init(float [] newBpms, float newBarBeat, GameObject obj)
	{
		this.bpms = newBpms;
		this.barBeat = newBarBeat;
		this.padPrefab = obj;
		this.selfColor = this.gameObject.GetComponent<MeshRenderer> ().material.color;
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
		
	}
}

[Serializable]
public class PadContainer
{
	private IPadController padController = null;

	public PadContainer(IPadController padController)
	{
		this.padController = padController;
	}
}
