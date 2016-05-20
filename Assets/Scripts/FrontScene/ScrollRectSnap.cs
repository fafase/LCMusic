using UnityEngine;
using System;
using System.Collections.Generic;

public interface IScrollRectSnap
{
    int ButtonLength {  get;  }
    float Center { get;  }
    float SnapSpeed { get; }
    IEnumerable<ScrollButton> RectTrs { get; }
	float MinSize{ get; }
}
public class ScrollRectSnap : MonoBehaviour, IScrollRectSnap
{
    [SerializeField]
    [Tooltip("The speed of the snapping")]
    private float snapSpeed = 5f;
    public float SnapSpeed { get { return this.snapSpeed; } }

    [SerializeField]
    [Tooltip ("The minimum size of the button while not centered")]
    [Range(0.0f,1.0f)]
    private float minSize;

	public float MinSize { get {return this.minSize; } }


    [SerializeField]
    private RectTransform panel = null;

    private ICollection<ScrollButton> bttn = null;
    public int ButtonLength { get { return this.bttn.Count; } }
    public IEnumerable<ScrollButton> RectTrs { get { return this.bttn as IEnumerable<ScrollButton>; } }

    [SerializeField]
    private RectTransform center = null;
    public float Center { get { return this.center.position.x; } }

    private ScrollRectContainer scrollRectContainer = null;

    private void Start()
    {
        this.bttn = this.gameObject.GetComponentsInChildren<ScrollButton>() as ICollection<ScrollButton>;
        foreach (ScrollButton sb in this.bttn)
        {
            sb.Init(this as IScrollRectSnap);
        }
        if (this.bttn == null) { throw new EmptyCollectionException("Scroll buttons"); }
        this.scrollRectContainer = new ScrollRectContainer(this as IScrollRectSnap);
		this.scrollRectContainer.RunUpdate(panel.anchoredPosition.x, SetPanel);
    }

    private void Update()
    {
        this.scrollRectContainer.RunUpdate(panel.anchoredPosition.x, SetPanel);
    }
    private void SetPanel(float result)
    {
        panel.anchoredPosition = new Vector2(result, panel.anchoredPosition.y);
    }

    public void StartDrag()
    {
        this.scrollRectContainer.Dragging = true;
    }
    public void EndDrag()
    {
        this.scrollRectContainer.Dragging = false;
    }

    private static ICollection<RectTransform> GetButtonRectTransforms(GameObject gameObject)
    {
        var sbs = gameObject.GetComponentsInChildren<ScrollButton>();
        var rts = new RectTransform[sbs.Length];
        for (int i = 0; i < sbs.Length; i++)
        {
            rts[i] = sbs[i].GetComponent<RectTransform>();
        }
        return rts as ICollection<RectTransform>;
    }
}
[Serializable]
public class ScrollRectContainer
{
    private IScrollRectSnap scrollRectSnap = null;
    public bool Dragging { get; set; } 
    private float btnDistance = 0;
    private float realDistance = 0f;
  
    public ScrollRectContainer(IScrollRectSnap scrollRectSnap)
    {
        this.scrollRectSnap = scrollRectSnap;
        this.btnDistance = GetDistance();
        this.realDistance = GetRealDistance();
    }

    public void RunUpdate(float currentPositionX,  Action<float> result)
    {
        float distance = Mathf.Infinity;
        float index = 0f;
        float position = -1f;
        foreach (ScrollButton sb in this.scrollRectSnap.RectTrs)
        {
            float x = sb.transform.position.x;
            float dis = Mathf.Abs(this.scrollRectSnap.Center - x);
            sb.SetScale(dis, this.realDistance); 
            if (dis < distance)
            {
                distance = dis;
                position = index * btnDistance;
            }
            index += 1f;
        }
        if (Dragging == false)
        {
            float x = Mathf.Lerp(currentPositionX, position, Time.deltaTime * this.scrollRectSnap.SnapSpeed);
            result (x);
        }
    }

    private float GetRealDistance()
    {
        ScrollButton first = null, second = null;
        using (IEnumerator<ScrollButton> enumer = this.scrollRectSnap.RectTrs.GetEnumerator())
        {
            if (enumer.MoveNext() == true)
            {
                first = enumer.Current;
            }
            if (enumer.MoveNext() == true)
            {
                second = enumer.Current;
            }
        }
        if (second == null || first == null) { throw new EmptyCollectionException("Scroll buttons"); }
        return Mathf.Abs(second.transform.position.x - first.transform.position.x);
    }
    private float GetDistance()
    {
        ScrollButton first = null, second = null;
        using (IEnumerator<ScrollButton> enumer = this.scrollRectSnap.RectTrs.GetEnumerator())
        {
            if (enumer.MoveNext() == true)
            {
                first = enumer.Current;
            }
            if (enumer.MoveNext() == true)
            {
                second = enumer.Current;
            }
        }
        if (second == null || first == null) { throw new EmptyCollectionException("Scroll buttons"); }
        return Mathf.Abs(second.RectTr.anchoredPosition.x -  first.RectTr.anchoredPosition.x) * -1f;
    }
}

