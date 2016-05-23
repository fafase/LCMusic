using UnityEngine;
using System.Collections;
using System;

public interface IPadMovement
{
	void HandleChangeBpm (BpmArg bpmArg);
	void InitPadMovement(IPadController newPadCtrl, Color color, Transform containerPad, float speed);
	void TapOnPadSuccessful();
}
public class PadMovementController : MonoBehaviour, IPadMovement, IPoolObject
{
	#region IPoolObject implementation

	public void Init (){ }

	public GameObject Prefab { get; set; }
	private IPadController padCtrl = null;
	#endregion

	public void InitPadMovement(IPadController newPadCtrl, Color color, Transform newContainerPad, float speed)
	{
		this.padCtrl = newPadCtrl;
		this.gameObject.GetComponent<MeshRenderer>().material.color = color;
		this.padMovement.SetPoolItem(newContainerPad);
		this.padMovement.SetBpmOnPad(speed);
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
		this.padMovement.SetBpmOnPad (bpmArg.bpm);
	}

	private void SetPosition()
	{
		Vector3 position = this.transform.position;
		position.z -= this.padMovement.Speed * Time.deltaTime * this.padDistance;
		this.transform.position = position;
	}

	public void TapOnPadSuccessful()
	{
		this.padMovement.TapOnPadSuccessful();
	}

	private void OnTriggerEnter(Collider col)
	{
		if(this.padMovement.CollisionEnterWithEndPad(col) == true)
		{
			// send message to PadController that entered
			if(this.padCtrl == null) { return; }
			this.padCtrl.SetEndPadCollisionEnter(this as IPadMovement);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if(this.padMovement.CollisionExitWithEndPad(col) == true)
		{
			// Send message to PadController that exited
			if(this.padCtrl == null){ return; }
			this.padCtrl.SetEndPadCollisionExit(this as IPadMovement);
		}
	}
}
[Serializable]
public class PadMovementContainer
{
	private float speed = 60f;			
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

	public void SetBpmOnPad(float newBpm)
	{
		this.speed = newBpm;
	}

	public void TapOnPadSuccessful()
	{
		GameObject go = this.gameObject;
		this.pool.PushToPool(ref go, true, this.containerPad);
	}

	public bool CollisionEnterWithEndPad(Collider col)
	{
		if(col.gameObject.CompareTag("EndPad"))
		{
			return true;
		}
		return false;
	}

	public bool CollisionExitWithEndPad(Collider col)
	{
		if(col.gameObject.CompareTag("EndPad"))
		{
			GameObject go = this.gameObject;
			this.pool.PushToPool(ref go, true, this.containerPad);
			return true;
		}
		return false;
	}
}
