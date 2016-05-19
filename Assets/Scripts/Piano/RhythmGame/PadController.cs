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
	private PadContainer padContainer = null;
	private IRhythmController rhytmController = null;

	private float barBeat = 4f;
	public float BarBeat{ get { return this.barBeat; } }

	private Color selfColor = Color.black;

	private float [] bpms = null;
	private int currentIndex = 0;

	private float timer = 0f;
	private float count = 0f;

	private ObjectPool pool = null;
	private Transform container = null;
	private GameObject prefab = null;

	private void Awake()
	{
		this.padContainer = new PadContainer (this as IPadController);
	}
		
	public void Init(IRhythmController newRhythmController, float [] newBpms, float newBarBeat, ObjectPool newPool, GameObject newPrefab, Transform newContainer)
	{
		this.rhytmController = newRhythmController;
		this.bpms = newBpms;
		this.barBeat = newBarBeat;
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
		padMovement.Pool = this.pool;
		padMovement.ContainerPad = this.rhytmController.ContainerPad;
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
