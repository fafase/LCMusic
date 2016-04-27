using UnityEngine;
using System.Collections;

public class PianoKeyController : MonoBehaviour {

    [SerializeField]
    private string clipName;
    public string ClipName { get { return this.clipName; } }

    [SerializeField]
    private float pitch = 1.0f;
    public float Pitch { get { return this.pitch; } }
}
