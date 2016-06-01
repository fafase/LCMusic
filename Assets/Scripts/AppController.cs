using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using LCHelper;
using UnityEngine.UI;

public sealed class AppController : MonoBehaviour
{
    private static AppController instance = null;
    public static AppController Instance { get { return instance; } }
	[SerializeField] private Text messageText = null;
	[SerializeField] private GameObject messageObj = null;

    private void Awake()
    {
		PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
		StartAppController();
		if(this.messageObj == null) { throw new NullReferenceException("Missing Message object ref"); }
		if(this.messageText == null) { throw new NullReferenceException("Missing Message Text ref");}
		this.messageObj.SetActive(false);
    }
		

    private void HandleDownloadComplete(object sender, DownloadCompleteArgs arg)
    {
		if(arg == null){ return; } 
        DownloadController dlCtrl = FindObjectOfType<DownloadController>();
        if (dlCtrl != null)
        {
            dlCtrl.RaiseDownloadComplete -= HandleDownloadComplete;
        }
		if(arg.json == null)
		{
			string temp = PlayerPrefs.GetString(ConstString.JsonData, null);
			Debug.Log(temp);
			if(string.IsNullOrEmpty(temp) == true)
			{
				DisplayErrorWithMessage(" A connection is required \n on first run \n for this app to work ");
				return;
			}
		}
        SceneManager.LoadScene("FrontScene");
    }

	private void DisplayErrorWithMessage(string message)
	{
		this.messageObj.SetActive(true);
		this.messageText.text = message;
	}

	public void StartAppController()
	{
		this.messageObj.SetActive(false);
		DownloadController dlCtrl = FindObjectOfType<DownloadController>();
		if (dlCtrl != null)
		{
			dlCtrl.RaiseDownloadComplete += HandleDownloadComplete;
			dlCtrl.Init();
		}
	}
}
