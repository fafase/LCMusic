using UnityEngine;
using System.Collections;
using System;
using LCHelper;
using UnityEngine.UI;

public interface IRhythmController 
{
	IMetronome MetronomeObject { get; }
	GameObject PrefaBtn { get; }
	RectTransform Container { get; }
	Transform Table { get; }
	Material [] Materials { get; }
	GameObject PrefabPad { get; }
}

public class RhythmController : MonoBehaviour , IRhythmController
{
	[SerializeField] private Text styleName; 
	[SerializeField] private IMetronome metronome = null;
	[SerializeField] private GameObject prefaBtn = null;
	[SerializeField] private RectTransform container = null;
	[SerializeField] private Transform table = null;
	[SerializeField] private GameObject padPrefab = null;

	public Material[] materials;

	public Material [] Materials { get { return this.materials; } }

	public RectTransform Container { get { return this.container; }}
	public Transform Table { get { return this.table; } }
	public GameObject PrefaBtn { get { return this.prefaBtn; } }
	public GameObject PrefabPad { get { return this.padPrefab; } }
	public IMetronome MetronomeObject { get { return this.metronome; } }

	private RhythmContainer rhythmContainer = null;

	private void Start()
	{
		this.metronome = FindObjectOfType<Metronome> () as IMetronome;
		this.metronome.RaiseBpm += HandleBpm;

		this.rhythmContainer = new RhythmContainer (this as IRhythmController);
		this.styleName.text = this.rhythmContainer.CurrentLesson.name;
	}

	private void HandleBpm (object sender, BpmBeatArg arg)
	{
		this.metronome.RaiseBpm -= HandleBpm;
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
		CreateButtonsWithCurrentLesson ();
	}

	private void CreateButtonsWithCurrentLesson()
	{
		int amount = this.currentLesson.rhythm.Length;
		float scaleX = 10f / (float)amount; 
		float posX = -5f + scaleX / 2f;
		for (int i = 0; i < amount; i++)
		{
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate (this.rhythmController.PrefaBtn);
			RectTransform rtContainer = this.rhythmController.Container;
			obj.GetComponent<RectTransform> ().SetParent (rtContainer, false);
			obj.GetComponent<Image> ().color = this.btnColor [i];

			CreatePadCube (scaleX, posX, 0.0f, i);
			GameObject newObj = CreatePadCube (scaleX, posX, 20.0f, i);
			newObj.AddComponent<PadController> ().Init(this.currentLesson.rhythm[i].bpms, this.rhythmController.PrefabPad);
		}
	}

	private GameObject CreatePadCube(float scaleX, float posX, float posY, int i)
	{
		GameObject newCube = UnityEngine.GameObject.CreatePrimitive (PrimitiveType.Cube);
		newCube.name = "Cube_" + i.ToString ();
		newCube.transform.parent = this.rhythmController.Table;
		newCube.GetComponent<MeshRenderer> ().material = this.rhythmController.Materials [i];
		newCube.transform.localScale = new Vector3 (scaleX, 1f, 0.5f);
		float tempX = posX + (float)i * scaleX; 
		newCube.transform.localPosition = new Vector3 (tempX,0f,posY);
		return newCube;
	}
}


