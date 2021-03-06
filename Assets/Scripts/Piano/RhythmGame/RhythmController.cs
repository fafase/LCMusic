﻿using UnityEngine;
using System.Collections;
using System;
using LCHelper;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public interface IRhythmController 
{
	GameObject PrefaBtn { get; }
	RectTransform Container { get; }
	Transform Table { get; }
	Material [] Materials { get; }
	void GetPadControllers(IPadController [] pcs);
	ObjectPool Pool { get; }
	GameObject PrefabPad { get; }
	Transform ContainerPad { get; }
	RhythmAudioController AudioController { get; }
	void SetUI(string uiText, int bpm);
	void SetChallenges(int initialBpm);
	void ResetRhythmGame();
}

public interface IRhythmStreak
{
	void IncreaseStreak(int index);
	void ResetStreak();
}
[RequireComponent(typeof(UIRhythmController))]
[RequireComponent( typeof(RhythmAudioController), typeof(RhythmInputController), typeof(RhythmChallengeController))]
public class RhythmController : MonoBehaviour , IRhythmController, IRhythmStreak
{
	[SerializeField] private GameObject prefaBtn = null;
	[SerializeField] private RectTransform container = null;
	[SerializeField] private Transform table = null;
	[SerializeField] private GameObject padPrefab = null;
	[SerializeField] private GameObject pauseObject = null;

	private UIRhythmController uiRhythmController = null;
	private RhythmAudioController audioController = null;
	public RhythmAudioController AudioController { get { return this.audioController; } }
	public GameObject PrefabPad { get{ return this.padPrefab; } }

	private IPause pause = null;

	public Material[] materials;
	public Material [] Materials { get { return this.materials; } }

	public RectTransform Container { get { return this.container; }}
	public Transform ContainerPad { get { return this.transform; } }
	public Transform Table { get { return this.table; } }
	public GameObject PrefaBtn { get { return this.prefaBtn; } }

	private RhythmContainer rhythmContainer = null;
	private StreakContainer streakContainer = null;

	private ObjectPool pool = null;
	public ObjectPool Pool { get { return this.pool; } }

	private IBeatCounter beatCounter = null;
	private RhythmInputController inputController = null;

	private RhythmChallengeController challengeController = null;
	private Lesson currentLesson = null;
	private IPadController [] pads = null;

	private void Awake()
	{
		if(this.pauseObject == null){ throw new NullReferenceException("Missing Pause object ref"); } 
		this.pause = this.pauseObject.GetComponent<IPause>();
		this.pause.RaisePause += Pause_RaisePause;
		SetObjectPool();

		this.uiRhythmController = this.gameObject.GetComponent<UIRhythmController>();
		this.audioController = this.gameObject.GetComponent<RhythmAudioController>();
		this.inputController = this.gameObject.GetComponent<RhythmInputController>();
		this.challengeController = this.gameObject.GetComponent<RhythmChallengeController>();
		this.inputController.enabled = false;
		this.beatCounter = this.gameObject.GetComponent<IBeatCounter>();
		if(beatCounter == null) { throw new NullReferenceException("Missing IBeatCounter"); }

		this.rhythmContainer = new RhythmContainer (this as IRhythmController, this as IRhythmStreak);
		this.pads = this.rhythmContainer.CreateButtonsWithCurrentLesson();

		GetPadControllers(pads);

		Lesson currentLesson = this.rhythmContainer.CurrentLesson;
		Rhythm rhythm = currentLesson.rhythm;
		SetUI(rhythm.IntroText, rhythm.bpmChallenge);
		foreach(IPadController pad in pads){ pad.SetBPM(rhythm.bpmChallenge);}
		this.streakContainer = new StreakContainer();

		this.uiRhythmController.SetStreakText(this.streakContainer.StreakCount.ToString());
		this.uiRhythmController.SetStyleNameText(currentLesson.name);

		this.beatCounter.Init(currentLesson.rhythm.bar);
		this.beatCounter.SetBeatCounterRunning(false);
		this.beatCounter.SetBpm(rhythm.bpmChallenge);

		this.challengeController.InitWithChallenges(rhythm.streakChallenge, rhythm.bpmChallenge);
	}

	private void OnDestroy()
	{
		this.pool.DeletePool();
		if(this.pause != null)
		{
			this.pause.RaisePause -= Pause_RaisePause;
		}
	}

	private void Pause_RaisePause (object sender, PauseEventArgs e)
	{
		Time.timeScale = (e.isPaused == true) ? 0.0f : 1.0f ;
	}

	public void GetPadControllers(IPadController [] pcs)
	{
		BeatCounter beatCounter = this.gameObject.GetComponent<BeatCounter>();
		if(beatCounter == null) { throw new NullReferenceException("Missing IBeatCounter"); }

		beatCounter.GetPadControllers(pcs as IEnumerable<IPadController>);

		this.inputController.Init(pcs);
	}

	private void SetObjectPool()
	{
		this.pool = ObjectPool.Instance;
		this.pool.AddToPool(this.padPrefab, 10, this.transform);
	}

	#region IRhythmStreak implementation

	public void IncreaseStreak (int index)
	{
		int streakCounter = this.streakContainer.IncreaseStreak();
		this.uiRhythmController.SetStreakText(streakCounter.ToString());
		int cs = this.challengeController.CheckCurrentChallenge(streakCounter);
		if(cs > 0)
		{
			SetUI(" Well done! \n You made it. ", 0);
			CleanActivePads();
			ResetRhythmGame();
		}
		this.audioController.PlayClipSuccess(index);
	}

	private void CleanActivePads()
	{
		IPoolObject[] objs = this.gameObject.GetComponentsInChildren<IPoolObject>();
		foreach(IPoolObject obj in objs)
		{
			GameObject o = obj.CurrentGO;
			this.pool.PushToPool(ref o, true, this.transform);
		}
	}
	public void ResetStreak ()
	{
		int streakCounter = this.streakContainer.ResetStreak();
		if(streakCounter != 0) 
		{
			throw new Exception("Streak not 0 on StreakReset");
		}
		this.uiRhythmController.SetStreakText(streakCounter.ToString());
		this.audioController.PlayBuzz();
	}

	#endregion

	public void StartRhythmGame()
	{
		this.beatCounter.SetBeatCounterRunning(true);	
		this.inputController.enabled = true;
	}

	public void ResetRhythmGame()
	{
		this.beatCounter.SetBeatCounterRunning(false);	
		this.inputController.enabled = false;
	}

	public void SetUI(string uiText, int bpm)
	{
		this.uiRhythmController.SetUI(uiText, bpm);
		foreach(IPadController pad in this.pads){ pad.SetBPM(bpm);}
	}

	public void SetChallenges(int initialBpm)
	{
		this.beatCounter.SetBpm(initialBpm);
	}
}

[Serializable]
public class RhythmContainer
{
	private Color[] btnColor = { Color.yellow, Color.blue, Color.green, Color.magenta };
	private IRhythmController rhythmController = null;
	private IRhythmStreak rhythmStreak = null;
	private Lesson currentLesson = null;
	public Lesson CurrentLesson{ get { return this.currentLesson; } }

	public RhythmContainer(IRhythmController rhythmController, IRhythmStreak rhythmStreak)
	{
		this.rhythmController = rhythmController;
		this.rhythmStreak = rhythmStreak;

		this.currentLesson = Save.DeserializeFromPlayerPrefs<Lesson> (ConstString.CurrentData);
	}

	public IPadController[] CreateButtonsWithCurrentLesson()
	{
		int amount = this.currentLesson.rhythm.beat.Length;
		this.rhythmController.AudioController.Init(amount);
		float scaleX = 10f / (float)amount; 
		float posX = -5f + scaleX / 2f;
		List<IPadController> list = new List<IPadController>();
		for (int i = 0; i < amount; i++)
		{
			GameObject btnObj = (GameObject)UnityEngine.Object.Instantiate (this.rhythmController.PrefaBtn);
			RectTransform rtContainer = this.rhythmController.Container;
			btnObj.GetComponent<RectTransform> ().SetParent (rtContainer, false);
			btnObj.GetComponent<Image> ().color = this.btnColor [i];

			CreateEndCube (scaleX, posX, i);
			GameObject newObj = CreateStartCube (scaleX, posX, i);
			PadController pc = newObj.AddComponent<PadController> ();
			pc.Init(this.rhythmController, this.rhythmStreak, i,
				this.currentLesson.rhythm.beat[i].bpms,
				this.currentLesson.rhythm.bar,
				this.rhythmController.Pool,
				this.rhythmController.PrefabPad, 
				this.rhythmController.ContainerPad);
			list.Add(pc as IPadController);
		}
		return list.ToArray();
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

	private void CreateEndCube(float scaleX, float posX, int i)
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
		newCube.transform.localPosition = new Vector3 (tempX,0f,0.0f);
	}

	private GameObject CreateStartCube(float scaleX, float posX, int i)
	{
		GameObject newCube = UnityEngine.GameObject.CreatePrimitive (PrimitiveType.Cube);
		newCube.name = "StartCube" + i.ToString ();
		newCube.transform.parent = this.rhythmController.Table;
		newCube.GetComponent<MeshRenderer> ().material = this.rhythmController.Materials [i];
		newCube.transform.localScale = new Vector3 (scaleX, 1f, 0.5f);
		float tempX = posX + (float)i * scaleX; 
		newCube.transform.localPosition = new Vector3 (tempX,0f,20.0f);
		return newCube;
	}
}

[Serializable]
public class StreakContainer
{
	public int StreakCount { get; private set;}
	public StreakContainer(){}

	public int IncreaseStreak()
	{
		this.StreakCount++;
		return this.StreakCount;
	}
	public int ResetStreak()
	{
		this.StreakCount = 0;
		return this.StreakCount;
	}
}

public static class PlayerPrefsWithAds
{
	public static bool CheckPlayerPrefsForLessonWithId(string id)
	{
		if(PlayerPrefs.HasKey(id) == false){ return false; }
		return true;
	}

	public static void SetPlayerPrefsForLessonWithId(string id, DateTime dateTime)
	{
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();
		LessonStorage ls = new LessonStorage(id, dateTime);
		bf.Serialize(ms, ls as LessonStorage);
		PlayerPrefs.SetString(id,Convert.ToBase64String(ms.GetBuffer()));
	}

	public static bool CheckDateTimeForLessonWithId(string id)
	{
		if(CheckPlayerPrefsForLessonWithId(id) == false)
		{
			return false;
		}
		string data = PlayerPrefs.GetString(id,null);
		if(data == null) { return false; }
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));
		LessonStorage ls = bf.Deserialize(ms) as LessonStorage;
		DateTime lessonTime = ls.LessonDateTime;
		TimeSpan ts = DateTime.Now.Subtract(lessonTime);
		if(ts.TotalMinutes > 120f)
		{
			PlayerPrefs.DeleteKey(id);
			return false;
		}
		return true;
	}

	[Serializable]
	class LessonStorage
	{
		public readonly string id = null;
		public readonly string dateTime = null;
		public DateTime LessonDateTime{ get { return DateTime.Parse(this.dateTime); } }

		public LessonStorage(string id, DateTime dateTime)
		{
			this.id = id;
			this.dateTime = dateTime.ToString();
		}
	}
}


