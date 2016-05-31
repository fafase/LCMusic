using UnityEngine;
using System.Collections;
using System;

public class RhythmInputController : MonoBehaviour 
{
	[SerializeField] GameObject pauseObject = null;
	private IPause pause = null;
	private IPadController[] padCtrls = null;
	private float screenHeight = 0.0f;
	private bool isPaused = false;

	private void Awake()
	{
		if(this.pauseObject == null){ throw new NullReferenceException("Missing Pause object ref"); }
		this.pause = this.pauseObject.GetComponent<IPause>();
		this.pause.RaisePause += Pause_RaisePause;

		this.screenHeight = Screen.width * 0.3f;
	}

	private void OnDestroy()
	{
		if(this.pause != null)
		{
			this.pause.RaisePause -= Pause_RaisePause;
		}
	}

	private void Pause_RaisePause (object sender, PauseEventArgs e)
	{
		this.isPaused = e.isPaused;
	}

	public void Init(IPadController[] pads)	
	{
		this.padCtrls = pads;
	}

	void Update () 
	{
		if(this.isPaused == true){ return; }
		int index = -1;
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0) == true)
		{
			float x = Input.mousePosition.x;
			float y = Input.mousePosition.y;
			index = CheckInput(x,y);
			if(index < 0) { return; }
			this.padCtrls[index].OnPointerEnter();
		}
		#elif UNITY_IOS || UNITY_ANDROID
		if(Input.touchCount > 0)
		{
		foreach (Touch touch in Input.touches)
		{
		if(touch.phase != TouchPhase.Began)
		{
		continue;
		}
		index = CheckInput(touch.position.x, touch.position.y);
		if(index < 0) { return; }
		this.padCtrls[index].OnPointerEnter();
		}
		}
		#endif
	}

	private int CheckInput(float x, float y)
	{
		if(y > screenHeight) { return -1; }

		float division = Screen.width / (float)padCtrls.Length;
		int index = (int)(x / division);
		return index;
	}
}
