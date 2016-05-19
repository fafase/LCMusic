using UnityEngine;
using System.Collections;
using System;

public interface IPadMovement
{
	void HandleChangeBpm (BpmArg bpmArg);
	void InitPadMovement(Color color, Transform containerPad);
}
public class PadMovementController : MonoBehaviour, IPadMovement, IPoolObject
{
	#region IPoolObject implementation

	public void Init (){ }

	public GameObject Prefab { get; set; }

	#endregion

	public void InitPadMovement(Color color, Transform newContainerPad)
	{
		this.gameObject.GetComponent<MeshRenderer>().material.color = color;
		this.padMovement.SetPoolItem(newContainerPad);
	}

	private PadMovementContainer padMovement = null;
	private float padDistance = 5f;

	private void Awake()
	{
		this.padMovement = new PadMovementContainer ( this.gameObject, ObjectPool.Instance);
	}

	private void Update()
	{
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

	private void OnTriggerEnter(Collider col)
	{
		this.padMovement.CollisionEnter(col);
	}
	private void OnTriggerExit(Collider col)
	{
		this.padMovement.CollisionExit(col);
	}
}
[Serializable]
public class PadMovementContainer
{
	private float speed = 60f;			// Level always starts on 60
	public float Speed { get { return this.speed / 60f; } }
	private GameObject gameObject = null;
	private ObjectPool pool = null;
	private Transform containerPad = null;

	public PadMovementContainer( GameObject newGameObject, ObjectPool pool)
	{
		this.gameObject = newGameObject;
		this.pool = pool;
	}

	public void SetPoolItem(Transform newContainerPad)
	{
		this.containerPad = newContainerPad;	
	}

	public void ChangeBpm(float newBpm)
	{
		this.speed = newBpm;
	}

	public void CollisionEnter(Collider col)
	{
		if(col.gameObject.CompareTag("EndPad"))
		{
			// Send Message to PadController
		}
	}
	public void CollisionExit(Collider col)
	{
		if(col.gameObject.CompareTag("EndPad"))
		{
			// Send Message to PadController
			GameObject go = this.gameObject;
			this.pool.PushToPool(ref go, true, this.containerPad);

		}
	}
}
