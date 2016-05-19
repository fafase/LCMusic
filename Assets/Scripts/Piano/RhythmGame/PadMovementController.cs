using UnityEngine;
using System.Collections;
using System;

public interface IPadMovement
{
	void HandleChangeBpm (BpmArg bpmArg);
	ObjectPool Pool { get; set; }
	Transform ContainerPad { get; set; }
}
public class PadMovementController : MonoBehaviour, IPadMovement, IPoolObject
{
	#region IPoolObject implementation

	public void Init () 
	{
		
	}

	public GameObject Prefab { get; set; }

	#endregion

	private PadMovementContainer padMovement = null;
	private float padDistance = 5f;

	private void Awake()
	{
		this.padMovement = new PadMovementContainer (this as IPadMovement, this.gameObject);
	}

	private void Update()
	{
		SetPosition ();
	}
		
	public ObjectPool Pool { get; set; }
	private Transform containerPad = null;
	public Transform ContainerPad { get; set; }
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

	private void OnTriggerEnter(Collider col)
	{
		this.padMovement.CollisionEnter(col);
	}
}
[Serializable]
public class PadMovementContainer
{
	private IPadMovement padMovement = null;
	private float speed = 60f;			// Level always starts on 60
	public float Speed { get { return this.speed / 60f; } }
	private GameObject gameObject = null;

	public PadMovementContainer(IPadMovement padMovement, GameObject newGameObject)
	{
		this.padMovement = padMovement;
		this.gameObject = newGameObject;
	}

	public void ChangeBpm(float newBpm)
	{
		this.speed = newBpm;
	}

	public void CollisionEnter(Collider col)
	{
		if(col.gameObject.CompareTag("EndPad"))
		{
			GameObject go = this.gameObject;
			this.padMovement.Pool.PushToPool(ref go, true, this.padMovement.ContainerPad);
		}
	}
}
