using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler
{
	private PadController padController = null;
	
	public void InitWithPadController(PadController newPadController)
	{
		this.padController = newPadController;
	}
	private void OnDestroy()
	{
		this.padController = null;
	}

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(this.padController == null){ return; }
		this.padController.OnPointerEnter();
	}

	#endregion
}
