using UnityEngine;
using System.Collections;
using System;

public interface IPadMovement
{
	void HandleChangeBpm (BpmArg bpmArg);
	bool IsRunning { get; set; }
}
public class PadMovementController : MonoBehaviour,IPadMovement
{
	private PadMovementContainer padMovement = null;
	private float padDistance = 5f;
	public bool IsRunning { get; set;}

	private void Awake()
	{
		this.padMovement = new PadMovementContainer (this as IPadMovement);
	}

	private void Update()
	{
		if (this.IsRunning == false) { return; }
		SetPosition ();
	}
		
	public void HandleChangeBpm( BpmArg bpmArg)
	{
		this.padMovement.ChangeBpm (bpmArg.bpm);
	}

	private void SetPosition()
	{
		Vector3 position = this.transform.position;
		position.z -= this.padMovement.Speed * Time.deltaTime * this.padDistance;
		this.transform.position = position;
	}
}
[Serializable]
public class PadMovementContainer
{
	private IPadMovement padMovement = null;
	private float speed = 0f;
	public float Speed { get { return this.speed / 60f; } }

	public PadMovementContainer(IPadMovement padMovement)
	{
		this.padMovement = padMovement;
	}

	public void ChangeBpm(float newBpm)
	{
		this.speed = newBpm;
	}
}
