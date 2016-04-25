using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FrontUIAction : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
