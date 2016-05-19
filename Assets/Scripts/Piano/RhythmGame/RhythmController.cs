using UnityEngine;
using System.Collections;
using System;
using LCHelper;
using UnityEngine.UI;
using System.Collections.Generic;

public interface IRhythmController 
{
	GameObject PrefaBtn { get; }
	RectTransform Container { get; }
	Transform Table { get; }
	Material [] Materials { get; }
	void GetPadControllers(IEnumerable<IPadController> pcs);
	ObjectPool Pool { get; }
	GameObject PrefabPad { get; }
	Transform ContainerPad { get; }
}

public class RhythmController : MonoBehaviour , IRhythmController
{
	[SerializeField] private Text styleName; 
	[SerializeField] private GameObject prefaBtn = null;
	[SerializeField] private RectTransform container = null;
	[SerializeField] private Transform table = null;
	[SerializeField] private GameObject padPrefab = null;
	public GameObject PrefabPad { get{ return this.padPrefab; } }
	public Material[] materials;

	public Material [] Materials { get { return this.materials; } }

	public RectTransform Container { get { return this.container; }}
	public Transform ContainerPad { get { return this.transform; } }
	public Transform Table { get { return this.table; } }
	public GameObject PrefaBtn { get { return this.prefaBtn; } }

	private RhythmContainer rhythmContainer = null;

	private ObjectPool pool = null;
	public ObjectPool Pool { get { return this.pool; } }

	private void Awake()
	{
		SetObjectPool();
		
		this.rhythmContainer = new RhythmContainer (this as IRhythmController);
		Lesson currentLesson = this.rhythmContainer.CurrentLesson;
		this.styleName.text = currentLesson.name;
	
		BeatCounter beatCounter = this.gameObject.GetComponent<BeatCounter>();
		if(beatCounter == null) { throw new NullReferenceException("Missing IBeatCounter"); }
		beatCounter.Init(currentLesson.rhythm.bar);
	}
		
	public void GetPadControllers(IEnumerable<IPadController> pcs)
	{
		BeatCounter beatCounter = this.gameObject.GetComponent<BeatCounter>();
		if(beatCounter == null) { throw new NullReferenceException("Missing IBeatCounter"); }

		beatCounter.GetPadControllers(pcs);
	}

	private void SetObjectPool()
	{
		this.pool = ObjectPool.Instance;
		this.pool.AddToPool(this.padPrefab, 10, this.transform);
	}
}

[Serializable]
public class RhythmContainer
{
	private Color[] btnColor = { Color.yellow, Color.blue, Color.green, Color.magenta };
	private IRhythmController rhythmController = null;
	private Lesson currentLesson = null;
	public Lesson CurrentLesson{ get { return this.currentLesson; } }

	public RhythmContainer(IRhythmController rhythmController)
	{
		this.rhythmController = rhythmController;
		this.currentLesson = Save.DeserializeFromPlayerPrefs<Lesson> (ConstString.CurrentData);

		IEnumerable<IPadController> pcs = CreateButtonsWithCurrentLesson ();
		this.rhythmController.GetPadControllers(pcs);
	}

	private IEnumerable<IPadController> CreateButtonsWithCurrentLesson()
	{
		int amount = this.currentLesson.rhythm.beat.Length;
		float scaleX = 10f / (float)amount; 
		float posX = -5f + scaleX / 2f;
		IList<IPadController> list = new List<IPadController>();
		for (int i = 0; i < amount; i++)
		{
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate (this.rhythmController.PrefaBtn);
			RectTransform rtContainer = this.rhythmController.Container;
			obj.GetComponent<RectTransform> ().SetParent (rtContainer, false);
			obj.GetComponent<Image> ().color = this.btnColor [i];

			CreatePadCube (scaleX, posX, 0.0f, i);
			GameObject newObj = CreatePadCube (scaleX, posX, 20.0f, i);
			newObj.name = "StartCube_"+i.ToString();
			newObj.tag = "Player";
			PadController pc = newObj.AddComponent<PadController> ();
			pc.Init(this.rhythmController, 
				this.currentLesson.rhythm.beat[i].bpms,
				this.currentLesson.rhythm.bar,
				this.rhythmController.Pool,
				this.rhythmController.PrefabPad, 
				this.rhythmController.ContainerPad);
			list.Add(pc as IPadController);
		}
		return list as IEnumerable<IPadController>;
	}

	private GameObject CreatePadCube(float scaleX, float posX, float posY, int i)
	{
		GameObject newCube = UnityEngine.GameObject.CreatePrimitive (PrimitiveType.Cube);
		newCube.name = "EndPad_" + i.ToString ();
		newCube.tag = "EndPad";
		Rigidbody rig = newCube.AddComponent<Rigidbody>();
		rig.isKinematic = true;
		newCube.GetComponent<Collider>().isTrigger = true;
		newCube.transform.parent = this.rhythmController.Table;
		newCube.GetComponent<MeshRenderer> ().material = this.rhythmController.Materials [i];
		newCube.transform.localScale = new Vector3 (scaleX, 1f, 0.5f);
		float tempX = posX + (float)i * scaleX; 
		newCube.transform.localPosition = new Vector3 (tempX,0f,posY);
		return newCube;
	}
}


