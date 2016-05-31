using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public interface IPause 
{
	event EventHandler<PauseEventArgs>RaisePause;
}

public class PauseEventArgs : System.EventArgs
{
	public readonly bool isPaused = false;
	public readonly bool isQuit = false;
	public PauseEventArgs(bool isPaused, bool isQuit)
	{
		this.isPaused = isPaused;
		this.isQuit = isQuit;
	}
}
public class PauseController : MonoBehaviour , IPause
{
	#region IPause implementation
	public event EventHandler<PauseEventArgs> RaisePause;
	protected void OnPause(PauseEventArgs args)
	{
		if(RaisePause != null)
		{
			RaisePause(this, args);
		}
	}
	#endregion

	[SerializeField] private GameObject pauseMenu = null;
	[SerializeField] private GameObject pauseBtn = null;

	private void Awake()
	{
		if(this.pauseMenu == null){ throw new NullReferenceException("Missing PauseMenu ref"); }
		if(this.pauseBtn == null) { throw new NullReferenceException("Missing PauseBtn ref"); }
		this.pauseMenu.SetActive(false);
		this.pauseBtn.SetActive(true);
	}

	public void OpenPauseMenu()
	{
		this.pauseMenu.SetActive(true);
		this.pauseBtn.SetActive(false);
		OnPause(new PauseEventArgs(true, false));
	}

	public void QuitLevel(string level)
	{
		if(string.IsNullOrEmpty(level) == true) { throw new NullReferenceException("Empty string passe to method"); }
		SceneManager.LoadScene(level);
		OnPause(new PauseEventArgs(false, true));
	}

	public void RestartLevel()
	{
		string currentLevel = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentLevel);
		OnPause(new PauseEventArgs(false, false));
	}

	public void ResumeLevel()
	{
		this.pauseMenu.SetActive(false);
		this.pauseBtn.SetActive(true);
		OnPause(new PauseEventArgs(false, false));
	}
}
