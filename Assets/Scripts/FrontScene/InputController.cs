using UnityEngine;
using System.Collections;
using System;

public interface IInput { }
public class InputController : MonoBehaviour , IInput
{
    private InputContainer inputContainer = null;

    private void Awake()
    {
        this.inputContainer = new InputContainer(this as IInput);
    }


}

[Serializable]
public class InputContainer
{
    private IInput input = null;

    public InputContainer(IInput input)
    {
        this.input = input;
    }


}
