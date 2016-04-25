using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FretController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private StringController stringCtrl = null;
	private uint fret = 0;
	private void Start()
	{
		this.stringCtrl = this.transform.parent.GetComponent<StringController> ();
		if (this.stringCtrl == null) 
		{
			Debug.LogError ("[LCMusic] Missing string controller in parent");
			return;
		}
		this.fret = RetrieveFretNumberInName (); 
	}

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		this.stringCtrl.SetCurrentFret (0);
	}

	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData)
	{
		this.stringCtrl.SetCurrentFret (this.fret);
	}
	#endregion

	private uint RetrieveFretNumberInName()
	{
		string name = this.gameObject.name;
		char lastChar = name [name.Length - 2];
		uint result = uint.Parse(lastChar.ToString()) + 1;
		return result;
	}
	public uint Fret{ get {  return this.fret; }  }
}
