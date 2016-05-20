using UnityEngine;
using System.Collections;

public class RhythmInputController : MonoBehaviour 
{

	private IPadController[] padCtrls = null;
	private float screenHeight = 0.0f;

	private void Awake()
	{
		this.screenHeight = Screen.width * 0.3f;
	}
	public void Init(IPadController[] pads)	
	{
		this.padCtrls = pads;
	}

	void Update () 
	{
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
