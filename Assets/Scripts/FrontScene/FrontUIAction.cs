using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public interface IUIAction
{
    void LoadScene(string scene);
}

public class FrontUIAction : MonoBehaviour , IUIAction
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void ShowSequence() { }
}
